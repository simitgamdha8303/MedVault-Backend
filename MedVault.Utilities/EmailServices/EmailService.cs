namespace MedVault.Utilities.EmailServices;

using MailKit.Net.Smtp;
using MedVault.Models.Dtos;
using Microsoft.Extensions.Options;
using MimeKit;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendOtpAsync(string email, string otp)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("MedVault", _settings.User));
        message.To.Add(MailboxAddress.Parse(email));
        message.Subject = "Your OTP Code";

        message.Body = new TextPart("plain")
        {
            Text = $"Your OTP is {otp}. It expires in 5 minutes."
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.Host, _settings.Port, false);
        await client.AuthenticateAsync(_settings.User, _settings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
