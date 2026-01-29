namespace MedVault.Models.Dtos.RequestDtos;

using System.ComponentModel.DataAnnotations;
using MedVault.Models.Enums;

public class BookAppointmentRequest
{
    [Required]
    public int DoctorId { get; set; }

    [Required]
    public DateTime AppointmentDate { get; set; }

    [Required]
    public TimeSpan AppointmentTime { get; set; }

    [Required]
    public CheckupType CheckupType { get; set; }
}
