using System.ComponentModel.DataAnnotations;
namespace MedVault.Models.Dtos.RequestDtos;


public class HospitalCreateRequest
{
    [Required]
    [MaxLength(255)]
    public string Name { get; set; } = default!;
}
