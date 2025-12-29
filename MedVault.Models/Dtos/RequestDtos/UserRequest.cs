using System.ComponentModel.DataAnnotations;
using MedVault.Common.Messages;

namespace MedVault.Models.Dtos.RequestDtos;

public class UserRequest
{
    // public Role Role { get; set; }

    [Required(ErrorMessage = ValidationMessages.EmailRequired)]
    [EmailAddress(ErrorMessage = ValidationMessages.InvalidEmail)]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9._%+-]*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
ErrorMessage = ValidationMessages.EmailSpecialChar)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.FirstNameRequired)]
    [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z\s]+$", ErrorMessage = ValidationMessages.FirstNameOnlyLetters)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.FirstNameRequired)]
    [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z\s]+$", ErrorMessage = ValidationMessages.FirstNameOnlyLetters)]
    public string MiddleName { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.FirstNameRequired)]
    [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z\s]+$", ErrorMessage = ValidationMessages.FirstNameOnlyLetters)]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.MobileRequired)]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^[1-9]\d{9}$", ErrorMessage = ValidationMessages.ContactnumberFormat)]
    public string Mobile { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.PasswordRequired)]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = ValidationMessages.PasswordMinLength)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])\S{8,}$",
ErrorMessage = ValidationMessages.PasswordRequirements)]
    public string Password { get; set; } = null!;
}
