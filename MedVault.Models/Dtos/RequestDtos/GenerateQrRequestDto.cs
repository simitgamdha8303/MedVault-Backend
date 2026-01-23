namespace MedVault.Models.Dtos.RequestDtos;

public class GenerateQrRequestDto
{
    public int DoctorId { get; set; }
    public int ExpiryMinutes { get; set; } = 10;
}
