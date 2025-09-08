using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.Infrastructure.DTOs
{
    public class CustomerDTO
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


        [StringLength(50)]
        public string Country { get; set; }

        [StringLength(200)]
        public string State { get; set; }

        [StringLength(200)]
        public string City { get; set; }

        public string Address { get; set; }


        //for customers business details
        [Required]
        [StringLength(100)]
        public string BusinessType { get; set; }

        [Required]
        [StringLength(50)]
        public string BusinessCountry { get; set; }

        [StringLength(200)]
        public string BusinessState { get; set; }

        [StringLength(200)]
        public string BusinessCity { get; set; }

        public string BusinessAddress { get; set; }


        //for customer prof of business details
        [Required]
        [StringLength(200)]
        public string ProfofBusinessType { get; set; }

        [Required]
        public string RegistrationFileContent { get; set; }
        [Required]
        public string MemorandumFileContent { get; set; }
        [Required]
        public string LicenseFileContent { get; set; }
        [Required]
        public string TaxFileContent { get; set; }


    }
}
