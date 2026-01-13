namespace MedVault.Models.Dtos.RequestDtos;

using MedVault.Models.Enums;
using System.ComponentModel.DataAnnotations;

public class MedicalTimelineRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int PatientId { get; set; }

    public int? DoctorProfileId { get; set; }

    [MaxLength(255)]
    public string? DoctorName { get; set; }

    [Required]
    public CheckupType CheckupType { get; set; }

    [Required]
    public DateOnly EventDate { get; set; }

    [MaxLength(1000)]
    public string? Notes { get; set; }
}
