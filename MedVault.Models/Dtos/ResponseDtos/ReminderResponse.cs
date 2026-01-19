namespace MedVault.Models.Dtos.ResponseDtos;

public class ReminderResponse
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string ReminderType { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime ReminderTime { get; set; }
    public string RecurrenceType { get; set; } = null!;
    public int RecurrenceInterval { get; set; }
    public DateTime? RecurrenceEndDate { get; set; }
    public bool IsActive { get; set; }
}
