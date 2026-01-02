using System.ComponentModel.DataAnnotations;
using MedVault.Common.Messages;

namespace MedVault.Models.Dtos.RequestDtos;

public class UserRequest
{
    // public Role Role { get; set; }

    [Required(ErrorMessage = ValidationMessages.EMAIL_REQUIRED)]
    [EmailAddress(ErrorMessage = ValidationMessages.INVALID_EMAIL)]
    [DataType(DataType.EmailAddress)]
    [RegularExpression(@"^[a-zA-Z0-9][a-zA-Z0-9._%+-]*@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
ErrorMessage = ValidationMessages.EMAIL_SPECIAL_CHAR)]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.FIRST_NAME_REQUIRED)]
    [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z\s]+$", ErrorMessage = ValidationMessages.FIRST_NAME_ONLY_LETTERS)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.LAST_NAME_REQUIRED)]
    [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z\s]+$", ErrorMessage = ValidationMessages.LAST_NAME_ONLY_LETTERS)]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.MOBILE_REQUIRED)]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^[1-9]\d{9}$", ErrorMessage = ValidationMessages.CONTACT_NUMBER_FORMAT)]
    public string Mobile { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.PASSWORD_REQUIRED)]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = ValidationMessages.PASSWORD_MIN_LENGTH)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_])\S{8,}$",
ErrorMessage = ValidationMessages.PASSWORD_REQUIREMENTS)]
    public string Password { get; set; } = null!;
}
