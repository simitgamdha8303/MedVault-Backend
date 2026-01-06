namespace MedVault.Models.Dtos.ResponseDtos;

public class DoctorProfileResponse
{
    public int HospitalId { get; set; }
    public string Specialization { get; set; } = null!;
    public string LicenseNumber { get; set; } = null!;
}