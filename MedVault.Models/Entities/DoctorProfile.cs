namespace MedVault.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class DoctorProfile
{
    [Key]
    public int Id { get; set; }

    [Required, ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [Required, ForeignKey(nameof(Hospital))]
    public int HospitalId { get; set; }

    [Required, MaxLength(100)]
    public string Specialization { get; set; } = null!;

    [Required, MaxLength(100)]
    public string LicenseNumber { get; set; } = null!;

    [Required]
    public bool IsVarified { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation

    public User User { get; set; } = null!;
    public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public Hospital Hospital { get; set; } = null!;
}

