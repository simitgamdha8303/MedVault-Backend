namespace MedVault.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MedVault.Models.Enums;


public class Reminder
{
    [Key]
    public int Id { get; set; }

    [Required, ForeignKey(nameof(PatientProfile))]
    public int PatientId { get; set; }

    [Required, ForeignKey(nameof(ReminderType))]
    public int ReminderTypeId { get; set; }

    [Required, MaxLength(255)]
    public string Title { get; set; } = null!;

    [Required]
    public string Description { get; set; } = null!;

    [Required]
    public DateTime ReminderTime { get; set; }

    public RecurrenceType RecurrenceType { get; set; } = RecurrenceType.None;
    
    public int RecurrenceInterval { get; set; } = 1;

    public DateTime? RecurrenceEndDate { get; set; }

    [Required]
    public bool IsActive { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation

    public PatientProfile PatientProfile { get; set; } = null!;

    public ReminderType ReminderType { get; set; } = null!;
}

