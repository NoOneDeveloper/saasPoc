using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poc.EF.Context;
using Poc.Infrastructure.DTOs.SinginUpDTO;
using Poc.Infrastructure.Interfaces.IRepositories.Customer;
using Microsoft.EntityFrameworkCore;
using Poc.EF.Entities;
using Poc.Infrastructure.DTOs.Customer;
using System.Reflection.Metadata.Ecma335;
using Poc.Infrastructure.DTOs.Global;
using Poc.Common.StaticClasses;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Poc.Common.Enum;
using Microsoft.IdentityModel.Tokens;

namespace Poc.Implementation.Repositories.CustomerRepositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDBContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomerRepository(ApplicationDBContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task AddCustomerAsync(SignUpRequestDTO DTO)
        {
            var Entity = new SignUpRequest
            {
                Id = DTO.Id,
                FirstName = DTO.FirstName,
                LastName = DTO.LastName,
                Email = DTO.Email,
                Phone = DTO.Phone,
                Mobile = DTO.Mobile,
                Salt = DTO.Salt,
                Hash = DTO.Hash,
                CreatedDate = DTO.CreatedDate,
               
                Status = DTO.Status,
                Reason = DTO.Reason,
                ModifiedBy = DTO.ModifiedBy

            };
            await _db.SignUpRequests.AddAsync(Entity);
        }



        #region Getting customer Details For KYC Form
        public async Task<Result<CustomerKycDTO>> GetByIdAsync(Guid Id)
        {
            var result = new Result<CustomerKycDTO>();
            var customer = await _db.SignUpRequests
                            .AsNoTracking()
                            .Where (c => c.Id == Id)
                            .Select(c => new CustomerKycDTO
                            {
                                FirstName = c.FirstName,
                                LastName = c.LastName,
                                Email = c.Email,
                                Phone = c.Phone,
                                Mobile = c.Mobile,
                                Salt = c.Salt,
                                Hash = c.Hash,
                            }).FirstOrDefaultAsync();

             result.Data = customer;
             return result;
        }

        #endregion

        #region Method for to save data in  table for Kyc Form

        public async Task<Result<string>> AddCustomerKycAsync(CustomerKycDTO request)
        {
            var result = new Result<string>();
            using var transaction = await _db.Database.BeginTransactionAsync();
            var Salt = request.Salt = Convert.FromBase64String(request.SaltBase64);

            var Hash = request.Hash = Convert.FromBase64String(request.HashBase64);
            try
            {
                // 1. Create and save the customer
                var customerDTO = new Customer()
                {
                    Id = Guid.NewGuid(),
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Phone = request.Phone,
                    Mobile = request.Mobile,
                    Country = request.Country,
                    State = request.State,
                    City = request.City,
                    Address = request.Address,
                    Status = false,
                    CreatedDate = DateTime.UtcNow,
                    Salt  = Salt,
                    Hash = Hash
                };
                await _db.Customers.AddAsync(customerDTO);
                await _db.SaveChangesAsync();

                // 2. Create and save the CustomerBusiness entity
                var customerBusiness = new CustomerBusiness
                {
                    Id = Guid.NewGuid(),
                    Type = request.BusinessType,
                    Country = request.BusinessCountry,
                    State = request.BusinessState,
                    City = request.BusinessCity,
                    Address = request.BusinessAddress,
                    CustomerId = customerDTO.Id,
                    CeatedDate = DateTime.UtcNow
                };
                await _db.CustomerBusinesses.AddAsync(customerBusiness);
                await _db.SaveChangesAsync();


                // 3. File upload to server and save in database
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "assets/uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var proofs = new List<ProofOfBusinessDTO>();

                if (request.RegistrationFileContent != null)
                    proofs.Add(await SaveFileAndMapAsync(request.RegistrationFileContent, FilesType.Registration.ToString(), uploadsFolder));
                if (request.MemorandumFileContent != null)
                    proofs.Add(await SaveFileAndMapAsync(request.MemorandumFileContent, FilesType.MemorandumArticles.ToString(), uploadsFolder));
                if (request.LicenseFileContent != null)
                    proofs.Add(await SaveFileAndMapAsync(request.LicenseFileContent, FilesType.TradeLicense.ToString(), uploadsFolder));
                if (request.TaxFileContent != null)
                    proofs.Add(await SaveFileAndMapAsync(request.TaxFileContent, FilesType.TaxIdentification.ToString(), uploadsFolder));

                foreach (var proof in proofs)
                {
                    var entity = new ProofOfBusiness
                    {
                        Id = Guid.NewGuid(),
                        Type = proof.Type,
                        FileContent = proof.FileContent,
                        Status = false,
                        CustomerId = customerDTO.Id,
                        CeatedDate = DateTime.UtcNow,
                    };
                    _db.ProofOfBusinesses.Add(entity);
                }

                await _db.SaveChangesAsync();
                await transaction.CommitAsync();

                result.Success = true;
                result.Message = "Customer KYC details added successfully.";
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                result.Success = false;
                result.Message = $"Failed to add Customer KYC details: {ex.Message}";
                result.Data = null;
            }
            return result;
        }
        #endregion

        #region private fuction for saving images in root folder
        private async Task<ProofOfBusinessDTO> SaveFileAndMapAsync(IFormFile file, string type, string uploadsFolder)
        {
            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            return new ProofOfBusinessDTO
            {
                Type = type,
                FileContent = uniqueFileName,
                Status = false
            };
        }
        #endregion

        public async Task<SignUpRequestDTO?> GetAdminByEmailAsync(string email)
        {
            return await GetCustomerByEmailAsync(email);
        }

        public async Task<SignUpRequestDTO?> GetCustomerByEmailAsync(string email)
        {
            var entity = await _db.SignUpRequests.FirstOrDefaultAsync(c => c.Email == email);
            if (entity == null) return null;

            return new SignUpRequestDTO
            {
                Email = entity.Email,
                Salt = entity.Salt,
                Hash = entity.Hash
            };
        }

        #region
        public async Task<Result<List<CustomerResponseDTO>>> CustomersListAsync()
        {
            var result = new Result<List<CustomerResponseDTO>>();
            try
            {
                var customers = await _db.Customers
                    .AsNoTracking()
                    .Select(c => new CustomerResponseDTO
                    {
                        Id = c.Id,
                        FirstName = c.FirstName,
                        LastName = c.LastName,
                        Email = c.Email,
                        Phone = c.Phone,
                        Status = c.Status,
                        CreatedDate = c.CreatedDate,
                        Reason = c.Reason,
                    }).ToListAsync();

                result.Data = customers;

            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error retrieving customers: {ex.Message}";
                result.Data = null;
            }
                return result;
        }
        #endregion
        public async  Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

    }
}
