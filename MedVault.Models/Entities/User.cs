namespace MedVault.Models.Entities;
using MedVault.Models.Enums;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public Role Role { get; set; }

    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required, MaxLength(255)]
    public string FirstName { get; set; } = null!;

    [Required, MaxLength(255)]
    public string MiddleName { get; set; } = null!;

    [Required, MaxLength(255)]
    public string LastName { get; set; } = null!;

    [Required, Phone, MaxLength(20)]
    public string Mobile { get; set; } = null!;

    [Required, MaxLength(1000)]
    public string PasswordHash { get; set; } = null!;

    public bool? IsVerified { get; set; }

    public bool TwoFactorEnabled { get; set; } = false;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation

    public PatientProfile? PatientProfile { get; set; }

    public DoctorProfile? DoctorProfile { get; set; }
}


