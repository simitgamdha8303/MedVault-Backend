namespace MedVault.Models.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OtpVerification
{
    [Key]
    public int Id { get; set; }

    [Required, StringLength(6, MinimumLength = 6)]
    public string Otp { get; set; } = null!;

    [Required]
    public bool IsVerify { get; set; }

    [Required, ForeignKey(nameof(User))]
    public int UserId { get; set; }

    public User User { get; set; } = null!;
}


