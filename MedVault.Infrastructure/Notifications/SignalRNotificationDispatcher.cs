using Microsoft.AspNetCore.SignalR;
using MedVault.Infrastructure.Hubs;
using MedVault.Models.Entities;
using MedVault.Utilities.EmailServices;
using MedVault.Data.IRepositories;


namespace MedVault.Infrastructure.Notifications;

public class SignalRNotificationDispatcher(IHubContext<NotificationHub> hubContext, IEmailService emailService, IUserRepository userRepository) : INotificationDispatcher
{
    public async Task SendReminderAsync(Reminder reminder)
    {
        if (reminder == null)
            throw new ArgumentNullException(nameof(reminder));

        int userId = reminder.PatientProfile.UserId;

        await hubContext.Clients
            .Group(userId.ToString())
            .SendAsync("ReminderNotification", new
            {
                reminderId = reminder.Id,
                title = reminder.Title,
                message = reminder.Description,
                time = reminder.ReminderTime
            });

        User? user = await userRepository.GetByIdAsync(userId);
        if (user == null)
            return;

        if (string.IsNullOrWhiteSpace(user.Email))
            throw new InvalidOperationException("Email is null or empty.");

        await emailService.SendReminderAsync(user.Email, reminder);
    }

}
