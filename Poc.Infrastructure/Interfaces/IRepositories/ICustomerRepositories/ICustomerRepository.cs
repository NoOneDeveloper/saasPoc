using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poc.Infrastructure.DTOs.Customer;
using Poc.Infrastructure.DTOs.Global;
using Poc.Infrastructure.DTOs.SinginUpDTO;

namespace Poc.Infrastructure.Interfaces.IRepositories.Customer
{
    public interface ICustomerRepository
    {
        Task AddCustomerAsync(SignUpRequestDTO DTO);
        Task SaveChangesAsync();
        Task<Result<CustomerKycDTO>> GetByIdAsync(Guid Id);
        Task<Result<string>> AddCustomerKycAsync(CustomerKycDTO request);

        Task<Result<List<CustomerResponseDTO>>> CustomersListAsync();
        Task<SignUpRequestDTO?> GetCustomerByEmailAsync(string email);
        Task<SignUpRequestDTO?> GetAdminByEmailAsync(string email);


    }
}

