using System.ComponentModel.DataAnnotations;
using MedVault.Common.Messages;

namespace MedVault.Models.Dtos.RequestDtos;

public class LoginRequest
{

    [Required(ErrorMessage = ValidationMessages.IDENTIFIER_REQUIRED)]
    public string Email { get; set; } = null!; // Email or Phone

    [Required(ErrorMessage = ValidationMessages.PASSWORD_REQUIRED)]
    public string Password { get; set; } = null!;
}
