namespace MedVault.Models.Dtos.ResponseDtos;

using MedVault.Models.Enums;

public class MedicalTimelineResponse
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public int? DoctorProfileId { get; set; }

    public string? DoctorName { get; set; }

    public CheckupType CheckupType { get; set; }

    public DateOnly EventDate { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }
}
