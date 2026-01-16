using System.ComponentModel.DataAnnotations;
using MedVault.Common.Messages;

namespace MedVault.Models.Dtos.RequestDtos;

public class UpdateUserProfileRequest
{
    // User
    [Required]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.FIRST_NAME_REQUIRED)]
    [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z\s]+$", ErrorMessage = ValidationMessages.FIRST_NAME_ONLY_LETTERS)]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.LAST_NAME_REQUIRED)]
    [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z\s]+$", ErrorMessage = ValidationMessages.LAST_NAME_ONLY_LETTERS)]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.MOBILE_REQUIRED)]
    [DataType(DataType.PhoneNumber)]
    [RegularExpression(@"^[1-9]\d{9}$", ErrorMessage = ValidationMessages.CONTACT_NUMBER_FORMAT)]
    public string Mobile { get; set; } = null!;

    // Doctor (nullable)
    [MaxLength(1000)]
    public string? Specialization { get; set; }

    [RegularExpression("^[A-Z]{2,3}[0-9]{5,7}$")]
    public string? LicenseNumber { get; set; }
    public int? HospitalId { get; set; }

    // Patient (nullable)
    public DateOnly? DateOfBirth { get; set; }
    public int? Gender { get; set; }
    public int? BloodGroup { get; set; }

    [MaxLength(500)]
    public string? Allergies { get; set; }

    [MaxLength(500)]
    public string? ChronicCondition { get; set; }

    [MaxLength(100)]
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
}
