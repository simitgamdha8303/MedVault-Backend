using System.ComponentModel.DataAnnotations;

namespace MedVault.Models.Dtos.RequestDtos;

public class GenerateQrRequest
{
    [Required]
    public int DoctorId { get; set; }
    public int ExpiryMinutes { get; set; } = 10;
}
