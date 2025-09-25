using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Poc.Infrastructure.Interfaces.IServices.Customer;

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
        [EmailExists]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(15, MinimumLength = 10, ErrorMessage = "Phone number must be between 10 and 15 digits")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number must contain only digits")]
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

        public bool? Status { get; set; } 

        public string? Reason { get; set; }

        public Guid? ModifiedBy { get; set; }

    }
}
public class EmailExistsAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            return ValidationResult.Success;

        var email = value.ToString();
        if (email.EndsWith("@pcipal.com", StringComparison.OrdinalIgnoreCase))
        {
            return new ValidationResult("Registration with @pcipal.com emails is not allowed.");
        }

        // Service fetch from DI container
        var customerService = (ICustomerService)validationContext.GetService(typeof(ICustomerService))!;
        if (customerService == null)
            throw new InvalidOperationException("ICustomerService not found in DI container.");

        // Check email exist
        var exists = customerService.IsEmailExistsAsync(email).GetAwaiter().GetResult();

        if (exists)
            return new ValidationResult("Email already exists.");

        return ValidationResult.Success;
    }
}