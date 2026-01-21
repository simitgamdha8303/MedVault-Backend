using System.ComponentModel.DataAnnotations;
using MedVault.Common.Messages;

namespace MedVault.Models.Dtos.RequestDtos;

public class DoctorProfileRequest
{

    [Required(ErrorMessage = ValidationMessages.HOSPITALID_REQUIRED)]
    public int HospitalId { get; set; }

    [Required(ErrorMessage = ValidationMessages.SPECIALIZATION_REQUIRED)]
    public string Specialization { get; set; } = null!;

    [Required(ErrorMessage = ValidationMessages.LICENSE_NUMBER_REQUIRED)]
    [RegularExpression("^[A-Z]{2,3}[0-9]{5,7}$",
         ErrorMessage = ValidationMessages.LICENSE_NUMBER_INVALID)]
    public string LicenseNumber { get; set; } = null!;
}