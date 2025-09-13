

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poc.Common.StaticClasses;
using Poc.EF.Context;
using Poc.Infrastructure.DTOs.SigninDTO;
using Poc.Infrastructure.DTOs.Customer;
using Poc.Infrastructure.DTOs.Global;
using Poc.Infrastructure.DTOs.SinginUpDTO;
using Poc.Infrastructure.Interfaces.IRepositories.Customer;
using Poc.Infrastructure.Interfaces.IServices.Customer;
using Microsoft.AspNetCore.Http;

namespace Poc.Implementation.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerService(ICustomerRepository repo, IHttpContextAccessor httpContextAccessor)
        {
            _repo = repo;
            _httpContextAccessor = httpContextAccessor;
        }
        public async  Task CreateCustomerAsync(SignUpRequestDTO customer)
        {
            customer.Salt = PasswordHasher.GenerateSalt();
            customer.Hash = PasswordHasher.HashPassword(customer.Password, customer.Salt);
            customer.Id = Guid.NewGuid();
            customer.CreatedDate = DateTime.UtcNow;
            customer.Status = true;

            await _repo.AddCustomerAsync(customer);
            await _repo.SaveChangesAsync();

            await EmailHelper.SendEmailAsync(
                customer.Email!,
               "Account Created!",
                EmailTemplates.AccountCreated(customer.FirstName)
            );
        }

        #region Get customer by ID
        public async Task<Result<CustomerKycDTO>> GetCustomer(Guid Id)
        {
            var customer = await _repo.GetByIdAsync(Id);
            if (!customer.Success)
            {
               customer.Success = false;
               customer.Message = "Customer not found";
            }
            return customer;
        }
        #endregion

        #region Add Customer KYC
        public async Task<Result<string>> AddCustomer(CustomerKycDTO customerKycDTO)
        {           
            var request = await _repo.AddCustomerKycAsync(customerKycDTO);

            if (!request.Success)
            {
                request.Success = false;
                request.Message = "Failed to add customer KYC";
            }

            return request;
        }
        #endregion

        #region Get List of Customers From repository
        public async Task<Result<List<CustomerResponseDTO>>> ListAsync()
        {
            var customers = await _repo.CustomersListAsync();

            if (!customers.Success || customers.Data == null || !customers.Data.Any())
            {
                return new Result<List<CustomerResponseDTO>>
                {
                    Success = false,
                    Message = "No customers found",
                    Data = new List<CustomerResponseDTO>() // safe empty list
                };
            }

            return customers;
        }
        #endregion

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var customer = await _repo.GetCustomerByEmailAsync(email);
            return customer != null;
        }

        public async Task<SignUpRequestDTO?> ValidateCustomerAsync(SiginDTO input)
        {
            var user = await _repo.GetCustomerByEmailAsync(input.Email);

            if (user == null)
                user = await _repo.GetAdminByEmailAsync(input.Email);

            if (user == null) return null;

            var valid = PasswordHasher.VerifyPassword(input.Password, user.Salt, user.Hash);
            if (!valid) return null;


            if (!string.IsNullOrEmpty(user.Email))
            {
                await EmailHelper.SendEmailAsync(
                    user.Email,
                    "Login Successful",
                    EmailTemplates.Welcome(user.FirstName)
                );
            }

            return user;
        }

        public async Task<bool> CheckEmailExistsAsync(string email)
        {
            return await _repo.EmailExistsAsync(email);
        }

        public async Task SendPasswordResetEmailAsync(string email)
        {
            // 1️⃣ Check if email exists
            bool exists = await _repo.EmailExistsAsync(email);
            if (!exists) return; // Email not found

            // 2️⃣ Generate reset token
            var token = Guid.NewGuid().ToString();

            // 2️⃣a Optional: Token expiration
            var expiration = DateTime.UtcNow.AddHours(1);

            // Optional: save token + expiration in DB if you want to track it
            // await _repo.SaveTokenAsync(email, token, expiration);

            // 3️⃣ Build reset link
            var resetLink = _httpContextAccessor.HttpContext.Request.Scheme + "://" +
                            _httpContextAccessor.HttpContext.Request.Host +
                            $"/Authentication/PasswordChangeBasic?token={token}&email={email}";

            // 4️⃣ Send email with expiration info
            await EmailHelper.SendEmailAsync(
                email,
                "Reset Your Password",
                EmailTemplates.PasswordReset("User", resetLink, expiration) // pass expiration
            );
        }

    }
}
 