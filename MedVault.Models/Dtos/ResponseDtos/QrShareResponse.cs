namespace MedVault.Models.Dtos.ResponseDtos;

public class QrShareResponse
{
    public Guid Id { get; set; }

    public string? DoctorName { get; set; }

    public DateTime ExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsUsed { get; set; }
}
