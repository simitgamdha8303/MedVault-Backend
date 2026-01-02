namespace MedVault.Models.Dtos.ResponseDtos;

public class LoginResponse
{
    public string? Token { get; set; }
    public int? UserId { get; set; }
    public bool RequiresOtp { get; set; }
}
