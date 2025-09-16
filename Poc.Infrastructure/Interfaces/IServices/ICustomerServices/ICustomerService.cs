


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poc.Infrastructure.DTOs.Customer;
using Poc.Infrastructure.DTOs.Global;
using Poc.Infrastructure.DTOs.SigninDTO;
using Poc.Infrastructure.DTOs.SinginUpDTO;

namespace Poc.Infrastructure.Interfaces.IServices.Customer
{
    public interface ICustomerService
    {
        Task CreateCustomerAsync(SignUpRequestDTO customer);
        Task<SignUpRequestDTO?> ValidateCustomerAsync(SiginDTO input);

        Task<bool> IsEmailExistsAsync(string email);

        Task<Result<string>> UpdateCustomerStatus(Guid customerId, bool status, Guid modifiedBy);
        Task<Result<CustomerKycDTO>> GetCustomer(Guid Id); 

        Task<Result<string>> AddCustomer(CustomerKycDTO customerKycDTO);
        Task<Result<List<SignUpCustomersDTO>>> ListAsync();


        Task<bool> CheckEmailExistsAsync(string email);
        Task SendPasswordResetEmailAsync(string email);

        Task<Result<CustomerDetailDTO>> GetCustomerDetailsAsync(Guid customerId);
    }
}
