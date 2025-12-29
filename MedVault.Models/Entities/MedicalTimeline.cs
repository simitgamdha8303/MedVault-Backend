namespace MedVault.Models.Entities;
using MedVault.Models.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class MedicalTimeline
{
    [Key]
    public int Id { get; set; }

    [Required, ForeignKey(nameof(PatientProfile))]
    public int PatientId { get; set; }

    [ForeignKey(nameof(DoctorProfile))]
    public int? DoctorProfileId { get; set; }

    [MaxLength(255)]
    public string? DoctorName { get; set; }

    [Required]
    public CheckupType CheckupType { get; set; }

    [Required]
    public DateOnly EventDate { get; set; }

    public string? Notes { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation

    public PatientProfile PatientProfile { get; set; } = null!;

    public DoctorProfile? DoctorProfile { get; set; }
}


