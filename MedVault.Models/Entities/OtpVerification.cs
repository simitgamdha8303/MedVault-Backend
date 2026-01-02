namespace MedVault.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OtpVerification
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(1000)]
    public string OtpHash { get; set; } = null!;

    [Required]
    public DateTime ExpiresAt { get; set; }

    [Required, ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public User User { get; set; } = null!;
}




