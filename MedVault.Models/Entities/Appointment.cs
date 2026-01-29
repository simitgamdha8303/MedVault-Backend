using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedVault.Models.Enums;

namespace MedVault.Models.Entities;

public class Appointment
{
    [Key]
    public int Id { get; set; }

    [Required, ForeignKey(nameof(PatientProfile))]
    public int PatientId { get; set; }

    [Required, ForeignKey(nameof(DoctorProfile))]
    public int DoctorId { get; set; }

    [Required]
    public DateTime AppointmentDate { get; set; }

    [Required]
    public CheckupType CheckupType { get; set; }

    [Required]
    public TimeSpan AppointmentTime { get; set; }

    [Required]
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;

    [Required]
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public PatientProfile PatientProfile { get; set; } = null!;

    public DoctorProfile DoctorProfile { get; set; } = null!;
}