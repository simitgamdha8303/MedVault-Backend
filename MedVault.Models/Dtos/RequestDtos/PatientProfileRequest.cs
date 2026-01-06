using MedVault.Models.Enums;

namespace MedVault.Models.Dtos.RequestDtos;

public class PatientProfileRequest
{
    public int UserId { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public BloodGroup BloodGroup { get; set; }
    public string? Allergies { get; set; }
    public string? ChronicCondition { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
}
