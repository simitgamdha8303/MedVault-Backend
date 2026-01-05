namespace MedVault.Models.Entities;

using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required, EmailAddress, MaxLength(255)]
    public string Email { get; set; } = null!;

    [Required, MaxLength(255)]
    public string FirstName { get; set; } = null!;

    [Required, MaxLength(255)]
    public string LastName { get; set; } = null!;

    [Required, Phone, MaxLength(20)]
    public string Mobile { get; set; } = null!;

    [Required, MaxLength(1000)]
    public string PasswordHash { get; set; } = null!;

    public bool? IsVerified { get; set; } = false;

    public bool TwoFactorEnabled { get; set; } = false;

    [Required]
    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    // Navigation
    
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public PatientProfile? PatientProfile { get; set; }

    public DoctorProfile? DoctorProfile { get; set; }
}


