using System.ComponentModel.DataAnnotations;
using MedVault.Models.Enums;

namespace MedVault.Models.Dtos.RequestDtos;

public class PatientProfileRequest
{
    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    [EnumDataType(typeof(Gender), ErrorMessage = "Invalid gender")]
    public Gender Gender { get; set; }

    [Required]
    [EnumDataType(typeof(BloodGroup), ErrorMessage = "Invalid blood group")]
    public BloodGroup BloodGroup { get; set; }

    [MaxLength(1000)]
    public string? Allergies { get; set; }

    [MaxLength(1000)]
    public string? ChronicCondition { get; set; }

    [MaxLength(1000)]
    public string? EmergencyContactName { get; set; }

    public string? EmergencyContactPhone { get; set; }
}
