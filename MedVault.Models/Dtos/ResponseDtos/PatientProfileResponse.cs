using MedVault.Models.Enums;

namespace MedVault.Models.Dtos.ResponseDtos;

public class PatientProfileResponse
{
    public DateOnly DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public BloodGroup BloodGroup { get; set; }
    public string? Allergies { get; set; }
    public string? ChronicCondition { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
}
