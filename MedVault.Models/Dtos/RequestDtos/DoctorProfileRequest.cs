using System.ComponentModel.DataAnnotations;

namespace MedVault.Models.Dtos.RequestDtos;

public class DoctorProfileRequest
{

    public int HospitalId { get; set; }

    public string Specialization { get; set; } = null!;

    [RegularExpression("^[A-Z]{2,3}[0-9]{5,7}$",
        ErrorMessage = "Invalid license number format")]
    public string LicenseNumber { get; set; } = null!;
}