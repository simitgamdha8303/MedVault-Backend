using System.ComponentModel.DataAnnotations;
using MedVault.Common.Messages;

namespace MedVault.Models.Dtos.RequestDtos;

public class LoginRequest
{

    [Required(ErrorMessage = ValidationMessages.IdentifierRequired)]
    public string Identifier { get; set; } = null!; // Email or Phone

    [Required(ErrorMessage = ValidationMessages.PasswordRequired)]
    public string Password { get; set; } = null!;
}
