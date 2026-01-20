using Hangfire;
using MedVault.Data.IRepositories;
using MedVault.Models.Entities;
using MedVault.Models.Enums;

public class ReminderJobService(
    IReminderRepository reminderRepository,
    INotificationDispatcher dispatcher,
    IBackgroundJobClient backgroundJobs)
{
    public async Task ExecuteAsync(int reminderId)
    {
        Reminder? reminder = await reminderRepository
            .GetReminderWithPatientAsync(reminderId);

        if (reminder == null || !reminder.IsActive)
            return;

        if (reminder.PatientProfile == null)
            return;

        await dispatcher.SendReminderAsync(reminder);

        // HANDLE RECURRENCE
        if (reminder.RecurrenceType == RecurrenceType.None)
        {
            reminder.IsActive = false;
        }
        else
        {
            DateTime? next = CalculateNextTime(reminder);

            if (next == null)
            {
                reminder.IsActive = false;
            }
            else
            {
                reminder.ReminderTime = next.Value;
                Schedule(reminder);
            }
        }

        await reminderRepository.SaveChangesAsync();
    }

    private static DateTime? CalculateNextTime(Reminder r)
    {
        DateTime next = r.RecurrenceType switch
        {
            RecurrenceType.Daily =>
                r.ReminderTime.AddDays(r.RecurrenceInterval),

            RecurrenceType.Weekly =>
                r.ReminderTime.AddDays(7 * r.RecurrenceInterval),

            _ => DateTime.MinValue
        };

        if (r.RecurrenceEndDate != null && next > r.RecurrenceEndDate)
            return null;

        return next;
    }

    private void Schedule(Reminder reminder)
    {
        backgroundJobs.Schedule<ReminderJobService>(
            j => j.ExecuteAsync(reminder.Id),
            reminder.ReminderTime
        );
    }
}
