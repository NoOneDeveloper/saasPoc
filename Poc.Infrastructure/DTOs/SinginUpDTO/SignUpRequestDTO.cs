using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poc.Infrastructure.DTOs.SinginUpDTO
{
    public class SignUpRequestDTO
    {
        [Key]
        public Guid Id { get; set; }

       [Required(ErrorMessage = "First Name is required")]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(500)]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string? Email { get; set; }

        [StringLength(15)]
        public string? Phone { get; set; }

        [StringLength(15)]
        public string? Mobile { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}$",
            ErrorMessage = "Password must be at least 8 characters long, contain uppercase, lowercase and a number")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string? ConfirmPassword { get; set; }


        [MaxLength(32)]
        public byte[]? Salt { get; set; }

   
        [MaxLength(64)]
        public byte[]? Hash { get; set; }

        public DateTime CreatedDate { get; set; }= DateTime.UtcNow;

        public DateTime? ModifiedDate { get; set; }

        public bool Status { get; set; } = false;

        public string? Reason { get; set; }

        public Guid? ModifiedBy { get; set; }

    }
}
