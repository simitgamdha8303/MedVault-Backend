namespace MedVault.Models.Entities;
using MedVault.Models.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class PatientProfile
{
    [Key]
    public int Id { get; set; }

    [Required, ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [Required]
    public DateOnly DateOfBirth { get; set; }

    [Required]
    public Gender Gender { get; set; }

    [Required]
    public BloodGroup BloodGroup { get; set; }

    public string? Allergies { get; set; }

    public string? ChronicCondition { get; set; }

    [MaxLength(255)]
    public string? EmergencyContactName { get; set; }

    [Phone, MaxLength(20)]
    public string? EmergencyContactPhone { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation

    public User User { get; set; } = null!;

    public ICollection<Document> Documents { get; set; } = new List<Document>();

    public ICollection<MedicalTimeline> MedicalTimelines { get; set; } = new List<MedicalTimeline>();

    public ICollection<Reminder> Reminder { get; set; } = new List<Reminder>();
}


