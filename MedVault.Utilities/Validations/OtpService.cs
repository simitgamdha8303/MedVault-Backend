namespace MedVault.Utilities.Validations;

public static class OtpGenerator
{
    public static string GenerateOtp()
    {
        return Random.Shared.Next(100000, 999999).ToString();
    }
}
