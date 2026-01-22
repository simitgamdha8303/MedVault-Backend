using System.ComponentModel.DataAnnotations;
using MedVault.Common.Messages;
using MedVault.Models.Enums;

namespace MedVault.Models.Dtos.RequestDtos;

public class PatientProfileRequest
{
    [Required]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [EnumDataType(typeof(Gender), ErrorMessage = ValidationMessages.INVALID_GENDER)]
    public Gender Gender { get; set; }

    [Required]
    [EnumDataType(typeof(BloodGroup), ErrorMessage = ValidationMessages.INVALID_BLOOD_GROUP)]
    public BloodGroup BloodGroup { get; set; }

    [MaxLength(1000)]
    public string? Allergies { get; set; }

    [MaxLength(1000)]
    public string? ChronicCondition { get; set; }

    [MaxLength(1000)]
    public string? EmergencyContactName { get; set; }

    public string? EmergencyContactPhone { get; set; }
}
