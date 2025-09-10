using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poc.Infrastructure.DTOs.SinginUpDTO;

namespace Poc.Infrastructure.Interfaces.IRepositories.Customer
{
    public interface ICustomerRepository

    {

        Task AddCustomerAsync(SignUpRequestDTO DTO);
        Task SaveChangesAsync();
        Task<SignUpRequestDTO?> GetCustomerByEmailAsync(string email);
    }
}
