namespace MedVault.Models.Dtos.RequestDtos;

using System.ComponentModel.DataAnnotations;

using MedVault.Models.Enums;

public class VerifyOtpRequest
{
    [Required(ErrorMessage = "UserId is required")]
    public int UserId { get; set; }
    
    [Required(ErrorMessage = "Otp is required")]
    public string Otp { get; set; } = null!;

    [Required(ErrorMessage = "Role is required")]
    public Role Role { get; set; }
}
