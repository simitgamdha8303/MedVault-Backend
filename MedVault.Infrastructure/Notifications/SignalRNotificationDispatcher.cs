using Microsoft.AspNetCore.SignalR;
using MedVault.Infrastructure.Hubs;
using MedVault.Models.Entities;


namespace MedVault.Infrastructure.Notifications;

public class SignalRNotificationDispatcher : INotificationDispatcher
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRNotificationDispatcher(
        IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendReminderAsync(Reminder reminder)
    {
        string userId = reminder.PatientProfile.UserId.ToString();

        await _hubContext.Clients
            .Group(userId)
            .SendAsync("ReminderNotification", new
            {
                reminderId = reminder.Id,
                title = reminder.Title,
                message = reminder.Description,
                time = reminder.ReminderTime
            });
    }
}
