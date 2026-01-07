using System.ComponentModel.DataAnnotations;
using MedVault.Models.Enums;

namespace MedVault.Models.Dtos.RequestDtos;

public class PatientProfileRequest
{
    [Required]
    public DateOnly DateOfBirth { get; set; }
    [Required]
    public Gender Gender { get; set; }
    [Required]
    public BloodGroup BloodGroup { get; set; }
    public string? Allergies { get; set; }
    public string? ChronicCondition { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
}
