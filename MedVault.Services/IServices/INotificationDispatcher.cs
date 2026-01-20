using MedVault.Models.Entities;

public interface INotificationDispatcher
{
    Task SendReminderAsync(Reminder reminder);
}
