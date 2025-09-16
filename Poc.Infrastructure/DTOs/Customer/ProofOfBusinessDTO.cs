using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Poc.Infrastructure.DTOs.Customer
{
    public class ProofOfBusinessDTO
    {
        [Required]
        [StringLength(200)]
        public int Type { get; set; }

        [Required]
        public string FileContent { get; set; }

        public bool? Status { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime CeatedDate { get; set; }

        public Guid? ModifiedBy { get; set; }

        public Guid CustomerId { get; set; }
        public string Reason { get; set; }
    }
}
