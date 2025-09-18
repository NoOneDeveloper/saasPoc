using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.Infrastructure.DTOs.Customer
{
    public class ProofActivityInputDTO
    {
        public string ProofId { get; set; }   // GUID as string
        public int? Type { get; set; }
        public string Reason { get; set; }
        public bool Status { get; set; }
    }
}
