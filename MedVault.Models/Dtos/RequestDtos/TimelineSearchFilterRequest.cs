using System.ComponentModel.DataAnnotations;
using MedVault.Models.Enums;

namespace MedVault.Models.Dtos.RequestDtos;

public class TimelineSearchFilterRequest
{

    public CheckupType? CheckupType { get; set; }

    [MaxLength(255)]
    public string? Doctor { get; set; }

    public DateTime? FromDate { get; set; }

    public DateTime? ToDate { get; set; }
}
