using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poc.Infrastructure.DTOs.Services;

namespace Poc.Infrastructure.DTOs.Requirement
{
    public class ServicesRequestDTO
    {
        public ServicesRequestDTO()
        {
            PaymentGatewayDTO = new List<PaymentGatewayDTO>();
        }
        public int SolutionType { get; set; }
        public int IntegrationType { get; set; }
        public int TransactionType { get; set; }
        public string PaymentGateway { get; set; }
        public List<PaymentGatewayDTO> PaymentGatewayDTO { get; set; }
    }
}
