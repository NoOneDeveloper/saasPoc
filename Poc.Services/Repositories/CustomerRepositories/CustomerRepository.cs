using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Poc.Common.Enum;
using Poc.EF.Context;
using Poc.EF.Entities;
using Poc.Infrastructure.DTOs.Customer;
using Poc.Infrastructure.DTOs.Global;
using Poc.Infrastructure.DTOs.SinginUpDTO;
using Poc.Infrastructure.Interfaces.IRepositories.Customer;

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
                            .Where(c => c.Id == Id)
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
                    Id = request.UserId,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    Phone = request.Phone,
                    Mobile = request.Mobile,
                    Country = request.Country,
                    State = request.State,
                    City = request.City,
                    Address = request.Address,
                    CreatedDate = DateTime.UtcNow,
                    Salt = Salt,
                    Hash = Hash
                };
                await _db.Customers.AddAsync(customerDTO);
                await _db.SaveChangesAsync();

                // 2. Create and save the CustomerBusiness entity
                var customerBusiness = new CustomerBusiness
                {
                    Id =Guid.NewGuid(),
                    Type = request.BusinessType,
                    Country = request.BusinessCountry,
                    State = request.BusinessState,
                    City = request.BusinessCity,
                    Address = request.BusinessAddress,
                    CustomerId = request.UserId,
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
                    proofs.Add(await SaveFileAndMapAsync(request.RegistrationFileContent, ((int)FilesType.Registration), uploadsFolder));
                if (request.MemorandumFileContent != null)
                    proofs.Add(await SaveFileAndMapAsync(request.MemorandumFileContent, ((int)FilesType.MemorandumArticles), uploadsFolder));
                if (request.LicenseFileContent != null)
                    proofs.Add(await SaveFileAndMapAsync(request.LicenseFileContent, ((int)FilesType.TradeLicense), uploadsFolder));
                if (request.TaxFileContent != null)
                    proofs.Add(await SaveFileAndMapAsync(request.TaxFileContent, ((int)FilesType.TaxIdentification), uploadsFolder));

                foreach (var proof in proofs)
                {
                    var entity = new ProofOfBusiness
                    {
                        Id = Guid.NewGuid(),
                        Type = proof.Type,
                        FileContent = proof.FileContent,
                        CustomerId = request.UserId,
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
        private async Task<ProofOfBusinessDTO> SaveFileAndMapAsync(IFormFile file, int type, string uploadsFolder)
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
            var entity = await _db.Users.FirstOrDefaultAsync(a => a.Email == email);
            if (entity == null) return null;

            return new SignUpRequestDTO
            {
                Id = entity.Id,
                Email = entity.Email,
                Salt = entity.PasswordSalt,
                Hash = entity.PasswordHash,

            };
            
        }

        public async Task<SignUpRequestDTO?> GetCustomerByEmailAsync(string email)
        {
            var entity = await _db.SignUpRequests.FirstOrDefaultAsync(c => c.Email == email);
            if (entity == null) return null;

            return new SignUpRequestDTO
            {    Id=entity.Id,
                Email = entity.Email,
                Salt = entity.Salt,
                Hash = entity.Hash
            };
        }

        #region getting cutomers list those who has not been approved yet
        public async Task<Result<List<SignUpCustomersDTO>>> CustomersListAsync()
        {
            var result = new Result<List<SignUpCustomersDTO>>();
            try
            {
                var customers = await _db.Customers
                    .AsNoTracking()
                    .Select(c => new SignUpCustomersDTO
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

        #region changestatus of cutomers
        public async Task<Result<string>> ChangeStatusAsync(StatusUpdateDTO request)
        {
            var result = new Result<string>();
            Guid customerId = Guid.Parse(request.Id);
            try
            {
                //var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
                var customer = await _db.SignUpRequests.FirstOrDefaultAsync(c => c.Id == customerId);
                if (customer == null)
                {
                    result.Success = false;
                    result.Message = "Customer not found.";
                    return result;
                }
                customer.Status = request.Status;
                customer.ModifiedDate = DateTime.UtcNow;
                _db.SignUpRequests.Update(customer);
                await _db.SaveChangesAsync();
                result.Success = true;
                result.Message = "Customer status updated successfully.";
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"Error updating customer status: {ex.Message}";
            }
            return result;
        }
        #endregion
        public async  Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _db.SignUpRequests.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> UpdateCustomerStatusAsync(Guid customerId, bool status, Guid modifiedBy)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
            if (customer == null)
                return false;

            customer.Status = status;
            customer.ModifiedBy = modifiedBy;      // 👈 Logged-in user id
            customer.ModifiedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async  Task<CustomerDetailDTO> GetCustomerDetailsAsync(Guid customerId)
        {
            var customerQuery =
         from c in _db.Customers
         where c.Id == customerId
         join b in _db.CustomerBusinesses on c.Id equals b.CustomerId into businessGroup
         from b in businessGroup.DefaultIfEmpty()
         select new CustomerDetailDTO
         {
             UserId = c.Id,
             FirstName = c.FirstName,
             LastName = c.LastName,
             Email = c.Email,
             Phone = c.Phone,
             Mobile = c.Mobile,
             Country = c.Country,
             State = c.State,
             City = c.City,
             Address = c.Address,

             BusinessType = b.Type,
             BusinessCountry = b.Country,
             BusinessState = b.State,
             BusinessCity = b.City,
             BusinessAddress = b.Address,


             ProofOfBusinesses = (from p in _db.ProofOfBusinesses
                                  where p.CustomerId == c.Id
                                  select new ProofOfBusinessDTO
                                  { Id = p.Id,
                                      Type = p.Type,
                                      TypeName = ((FilesType)p.Type).ToString(),
                                      FileContent = p.FileContent,
                                      Status = p.Status,
                                      CeatedDate = p.CeatedDate,
                                      ModifiedBy = p.ModifiedBy,
                                  }).ToList(),


             ProofOfBusinessesActivity = (from a in _db.ProofOfBusinessActivities
                                          where a.CustomerId == c.Id
                                          select new ProofofBusinessActivityDTO
                                          { Id = a.Id,
                                              Type = a.Type,
                                              TypeName = ((FilesType)a.Type).ToString(),
                                              Reason = a.Reason,
                                              CreatedDate = a.CreatedDate,
                                              Status = a.Status,
                                              ModifiedBy = a.ModifiedBy,
                                          }).ToList()
         };

            return await customerQuery.FirstOrDefaultAsync();
        }

        public async Task<Result<CustomerDetailDTO>> GetCustomerDetailsById(Guid customerId)
        {
            var response = new Result<CustomerDetailDTO>();
            try
            {
                var customerDetails = await (from c in _db.Customers
                                             join cb in _db.CustomerBusinesses on c.Id equals cb.CustomerId
                                             where c.Id == customerId
                                             select new CustomerDetailDTO
                                             {
                                                 UserId = c.Id,
                                                 FirstName = c.FirstName,
                                                 LastName = c.LastName,
                                                 Email = c.Email,
                                                 Phone = c.Phone,
                                                 Mobile = c.Mobile,
                                                 Country = c.Country,
                                                 State = c.State,
                                                 City = c.City,
                                                 Address = c.Address,
                                                 CreatedDate = c.CreatedDate,
                                                 ModifiedDate = c.ModifiedDate,
                                                 BusinessType = cb.Type,
                                                 BusinessCountry = cb.Country,
                                                 BusinessState = cb.State,
                                                 BusinessCity = cb.City,
                                                 BusinessAddress = cb.Address,
                                                 ProofOfBusinesses = _db.ProofOfBusinesses
                                                                        .Where(p => p.CustomerId == c.Id)
                                                                        .Select(p => new ProofOfBusinessDTO
                                                                        {
                                                                            Type = p.Type,
                                                                            FileContent = p.FileContent,
                                                                            Status = p.Status,
                                                                            CeatedDate = p.CeatedDate,
                                                                        }).ToList(),
                                                ProofOfBusinessesActivity = _db.ProofOfBusinessActivities
                                                                        .Where(p => p.CustomerId == c.Id)
                                                                        .Select(p => new ProofofBusinessActivityDTO
                                                                        {
                                                                            Type = p.Type,
                                                                            Reason = p.Reason,
                                                                            Status = p.Status,
                                                                            ModifiedBy = p.ModifiedBy,
                                                                        }).ToList()
                                             }).FirstOrDefaultAsync();

                response.Data = customerDetails;

            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error retrieving customer details: {ex.Message}";
                response.Data = null;
            }
            
            return response;
        }

        public async Task<bool> UpdateProofStatusAsync(Guid proofId, bool status, Guid modifiedBy)
        {
            var proof = await _db.ProofOfBusinesses.FirstOrDefaultAsync(p => p.Id == proofId);
            if (proof == null) return false;

            proof.Status = status;
            proof.ModifiedBy = modifiedBy;
            proof.CeatedDate = DateTime.Now;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task AddProofActivity(ProofofBusinessActivityDTO dto)
        {
            var entity = new ProofOfBusinessActivity
            {
                
                CustomerId = dto.CustomerId,
                Type = dto.Type,
                Reason = dto.Reason,
                Status = dto.Status,
                ModifiedBy = dto.ModifiedBy,
                CreatedDate = dto.CreatedDate ?? DateTime.UtcNow
            };

            await _db.ProofOfBusinessActivities.AddAsync(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<ProofOfBusinessDTO> GetProofById(Guid proofId)
        {
            var proof = await _db.ProofOfBusinesses.FindAsync(proofId);
            if (proof == null) return null;

            return new ProofOfBusinessDTO
            {
                Id = proof.Id,
                CustomerId = proof.CustomerId,
                Type = proof.Type,
                Status = proof.Status,
                FileContent = proof.FileContent
               
                 
            };
        }
    }
}
