using System.ComponentModel.DataAnnotations;

namespace MedVault.Models.Dtos.RequestDtos;

public class ResendOtpRequest
{
    [Required(ErrorMessage = "UserId is required")]
    
    public int UserId { get; set; }
}
