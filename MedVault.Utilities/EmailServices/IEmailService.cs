using MedVault.Models.Entities;

namespace MedVault.Utilities.EmailServices;

public interface IEmailService
{
    Task SendOtpAsync(string email, string otp);
    Task SendReminderAsync(string email, Reminder reminder);
}

