namespace MedVault.Models.Dtos.RequestDtos;

using System.ComponentModel.DataAnnotations;
using MedVault.Common.Messages;
using MedVault.Models.Enums;

public class VerifyOtpRequest
{
    [Required(ErrorMessage = ValidationMessages.USERID_REQUIRED)]
    public int UserId { get; set; }

    [Required(ErrorMessage = ValidationMessages.OTP_REQUIRED)]
    public string Otp { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.ROLE_REQUIRED)]
    public Role Role { get; set; }
}
