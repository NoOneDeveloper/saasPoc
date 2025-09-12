using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Poc.Common.RemoteValidation;

namespace Poc.Infrastructure.DTOs.Customer
{
    public class CustomerKycDTO
    {

        //for customers personl details 
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

        [MaxLength(32)]
        public byte[]? Salt { get; set; }

        [MaxLength(64)]
        public byte[]? Hash { get; set; }

        [Required]
        public string SaltBase64 { get; set; }
        [Required]
        public string HashBase64 { get; set; }


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

        [Required(ErrorMessage ="File is Required")]
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".pdf", ".HEIC" }, 5)]
        public IFormFile RegistrationFileContent { get; set; }

        [Required(ErrorMessage = "File is Required")]
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".pdf", ".HEIC" },5)]
        public IFormFile MemorandumFileContent { get; set; }

        [Required(ErrorMessage = "File is Required")]
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".pdf", ".HEIC" }, 5)]
        public IFormFile LicenseFileContent { get; set; }

        [Required(ErrorMessage = "File is Required")]
        [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png", ".pdf", ".HEIC" }, 5)]
        public IFormFile TaxFileContent { get; set; }

    }
}
