using System.ComponentModel.DataAnnotations;
using MedVault.Common.Messages;
using MedVault.Models.Enums;

namespace MedVault.Models.Dtos.RequestDtos;

public class UpdateReminderRequest
{
    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = null!;

    [Required]
    public DateTime ReminderTime { get; set; }

    [Required]
    public RecurrenceType RecurrenceType { get; set; }

    public int RecurrenceInterval { get; set; }

    public DateTime? RecurrenceEndDate { get; set; }

    public bool IsActive { get; set; }

    [Required]
    public int ReminderTypeId { get; set; }
}
