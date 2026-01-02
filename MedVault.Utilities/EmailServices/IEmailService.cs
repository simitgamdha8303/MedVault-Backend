namespace MedVault.Utilities.EmailServices;

public interface IEmailService
{
    Task SendOtpAsync(string email, string otp);
}

