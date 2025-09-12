using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.Infrastructure.DTOs.Customer
{
    public class CustomerResponseDTO
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Required]
        [StringLength(500)]
        public string? Email { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        public bool Status { get; set; } = false;

        public string? Reason { get; set; }

        public Guid? ModifiedBy { get; set; }
    }
}
