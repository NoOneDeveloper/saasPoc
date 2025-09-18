using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Poc.Infrastructure.DTOs.Customer
{
    public class ProofofBusinessActivityDTO
    {
        public Guid Id { get; set; }

        public int? Type { get; set; }

        [Required]
        public string Reason { get; set; }
        public string TypeName { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        public bool? Status { get; set; }

        public Guid ModifiedBy { get; set; }

        public Guid CustomerId { get; set; }
    }
       
}
