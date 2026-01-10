namespace MedVault.Models.Dtos.ResponseDtos;

public class UserProfileResponse
{
    // User
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Mobile { get; set; } = null!;
    public bool TwoFactorEnabled { get; set; }
    public string Role { get; set; } = null!;

    // Doctor (nullable)
    public DoctorProfileResponse? DoctorProfile { get; set; }

    // Patient (nullable)
    public PatientProfileResponse? PatientProfile { get; set; }
}