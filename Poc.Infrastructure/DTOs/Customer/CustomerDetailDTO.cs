using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Poc.Common.RemoteValidation;

namespace Poc.Infrastructure.DTOs.Customer
{
    public class CustomerDetailDTO
    {
        public Guid UserId { get; set; }

        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(320)]
        public string Email { get; set; }
         
        [StringLength(15)]
        public string Phone { get; set; }

        [StringLength(15)]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "Country is Required")]
        [StringLength(50)]
        public string Country { get; set; }

        [Required(ErrorMessage = "State is Required")]
        [StringLength(200)]
        public string State { get; set; }

        [Required(ErrorMessage = "City is Required")]
        [StringLength(200)]
        public string City { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public Guid? ModifiedBy { get; set; }

        [Required(ErrorMessage = "File is Required")]
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".pdf", ".HEIC" }, 5)]
        public IFormFile FileContent { get; set; }

        public int Type { get; set; }

        //for customers business details
        [Required(ErrorMessage = "Business Name is Required")]
        [StringLength(100)]
        public string BusinessType { get; set; }

        [Required(ErrorMessage = "Country is Required")]
        [StringLength(50)]
        public string BusinessCountry { get; set; }

        [Required(ErrorMessage = "State is Required")]
        [StringLength(200)]
        public string BusinessState { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(200)]
        public string BusinessCity { get; set; }

        [Required(ErrorMessage = "Address is Required")]
        public string BusinessAddress { get; set; }

        //for customer prof of business details
        public List<ProofOfBusinessDTO> ProofOfBusinesses = new();

        public List<ProofofBusinessActivityDTO> ProofOfBusinessesActivity = new();

        public ChangeFileDTO changeFileDTO { get; set; } = new();


    }
}
