using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poc.Infrastructure.DTOs.SinginUpDTO;

namespace Poc.Infrastructure.Interfaces.IServices.Customer
{
    public interface ICustomerService
    {
        Task CreateCustomerAsync(SignUpRequestDTO customer);
    }
}
