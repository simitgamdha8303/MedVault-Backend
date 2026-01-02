namespace MedVault.Models.Dtos.RequestDtos;

public class VerifyOtpRequest
{
    public int UserId { get; set; }
    public string Otp { get; set; } = null!;
}
