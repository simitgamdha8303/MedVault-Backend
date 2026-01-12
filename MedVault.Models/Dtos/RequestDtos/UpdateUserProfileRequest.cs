namespace MedVault.Models.Dtos.RequestDtos;

public class UpdateUserProfileRequest
{
    // User
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Mobile { get; set; } = null!;

    // Doctor (nullable)
    public string? Specialization { get; set; }
    public string? LicenseNumber { get; set; }
    public int? HospitalId { get; set; }

    // Patient (nullable)
    public DateOnly? DateOfBirth { get; set; }
    public int? Gender { get; set; }
    public int? BloodGroup { get; set; }
    public string? Allergies { get; set; }
    public string? ChronicCondition { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
}
