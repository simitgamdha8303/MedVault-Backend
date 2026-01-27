namespace MedVault.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class QrShare
{
    [Key]
    public Guid Id { get; set; }

    [Required, ForeignKey(nameof(PatientProfile))]
    public int PatientId { get; set; }

    [Required, ForeignKey(nameof(DoctorProfile))]
    public int DoctorId { get; set; }

    [Required]
    [Column(TypeName = "text")]
    public string AccessToken { get; set; } = null!;

    [Required]
    public DateTime ExpiresAt { get; set; }

    [Required]
    public bool IsUsed { get; set; } = false;

    public DateTime? UsedAt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation

    public PatientProfile PatientProfile { get; set; } = null!;

    public DoctorProfile DoctorProfile { get; set; } = null!;
}


