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

namespace Poc.Implementation.Repositories.CustomerRepositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDBContext _db;
        public CustomerRepository(ApplicationDBContext db)
        {
            _db = db;
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

        public async  Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
