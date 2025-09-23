using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.Infrastructure.DTOs.Services
{
    public class PaymentGatewayDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
