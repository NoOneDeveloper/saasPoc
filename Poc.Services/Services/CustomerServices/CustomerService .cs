

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poc.Common.StaticClasses;
using Poc.EF.Context;
using Poc.Infrastructure.DTOs.SigninDTO;
using Poc.Infrastructure.DTOs.SinginUpDTO;
using Poc.Infrastructure.Interfaces.IRepositories.Customer;
using Poc.Infrastructure.Interfaces.IServices.Customer;

namespace Poc.Implementation.Services.CustomerServices
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;
        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
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

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            var customer = await _repo.GetCustomerByEmailAsync(email);
            return customer != null;
        }

        public async Task<SignUpRequestDTO?> ValidateCustomerAsync(SiginDTO input)
        {
            var user = await _repo.GetCustomerByEmailAsync(input.Email);
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
    }
}
 