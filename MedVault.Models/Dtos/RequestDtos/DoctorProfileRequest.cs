namespace MedVault.Models.Dtos.RequestDtos;

public class DoctorProfileRequest
{
    public int UserId { get; set; }

    public int HospitalId { get; set; }

    public string Specialization { get; set; } = null!;

    public string LicenseNumber { get; set; } = null!;

    public bool IsVerified { get; set; }
}