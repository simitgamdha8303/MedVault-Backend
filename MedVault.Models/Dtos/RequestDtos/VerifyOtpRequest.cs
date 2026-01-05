namespace MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Enums;

public class VerifyOtpRequest
{
    public int UserId { get; set; }
    public string Otp { get; set; } = null!;
    public Role Role { get; set; }
}
