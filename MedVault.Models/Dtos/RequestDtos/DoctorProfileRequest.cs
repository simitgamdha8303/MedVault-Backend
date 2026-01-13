using System.ComponentModel.DataAnnotations;

namespace MedVault.Models.Dtos.RequestDtos;

public class DoctorProfileRequest
{

    [Required(ErrorMessage = "HospitalId is required")]
    public int HospitalId { get; set; }

    [Required(ErrorMessage = "Specialization is required")]
    public string Specialization { get; set; } = null!;

    [Required(ErrorMessage = "License number is required")]
    [RegularExpression("^[A-Z]{2,3}[0-9]{5,7}$",
         ErrorMessage = "Invalid license number format")]
    public string LicenseNumber { get; set; } = null!;
}