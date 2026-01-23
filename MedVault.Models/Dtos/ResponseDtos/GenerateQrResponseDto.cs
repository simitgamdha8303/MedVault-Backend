namespace MedVault.Models.Dtos.ResponseDtos;

public class GenerateQrResponseDto
{
    public string QrUrl { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}
