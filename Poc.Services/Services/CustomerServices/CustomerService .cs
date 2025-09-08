using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poc.Common.StaticClasses;
using Poc.EF.Context;
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
        }
    }
}
