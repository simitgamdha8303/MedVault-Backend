namespace MedVault.Models.Dtos.ResponseDtos;

public class QrShareByDoctorResponse
{
    public Guid Id { get; set; }

    public string? PatientName { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }
}
