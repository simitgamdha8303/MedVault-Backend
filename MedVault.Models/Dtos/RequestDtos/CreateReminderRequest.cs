using System.ComponentModel.DataAnnotations;
using MedVault.Common.Messages;
using MedVault.Models.Enums;

namespace MedVault.Models.Dtos.RequestDtos;

public class CreateReminderRequest
{
    [Required]
    public int ReminderTypeId { get; set; }

    [Required]
    [MaxLength(255)]
    public string Title { get; set; } = null!;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = null!;

    [Required]
    public DateTime ReminderTime { get; set; }

    public RecurrenceType RecurrenceType { get; set; } = RecurrenceType.None;

    [Range(1, 365, ErrorMessage = ValidationMessages.RECURRENCE_INTERVAL)]
    public int RecurrenceInterval { get; set; } = 1;

    public DateTime? RecurrenceEndDate { get; set; }
}
