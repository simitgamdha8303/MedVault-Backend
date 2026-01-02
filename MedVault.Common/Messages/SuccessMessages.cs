namespace MedVault.Common.Messages;

public static class SuccessMessages
{
    // public static string Registered(string entityName) => $"{entityName} registered successfully.";
    // public static string Updated(string entityName) => $"{entityName} updated successfully.";
    // public static string Deleted(string entityName) => $"{entityName} deleted successfully.";
    // public static string Restored(string entityName) => $"{entityName} restored successfully.";
    // public const string Retrieved = "Data retrieved successfully.";
    // public const string LoginSuccess = "Login successful.";
    // public const string OtpSent = "OTP sent successfully";
    // public const string OtpVerified = "OTP verified successfully.";

    private const string RegisteredTemplate = "{0} registered successfully.";
    private const string UpdatedTemplate = "{0} updated successfully.";
    private const string DeletedTemplate = "{0} deleted successfully.";
    private const string RestoredTemplate = "{0} restored successfully.";

    public static string Registered(string entityName) =>
        string.Format(RegisteredTemplate, entityName);

    public static string Updated(string entityName) =>
        string.Format(UpdatedTemplate, entityName);

    public static string Deleted(string entityName) =>
        string.Format(DeletedTemplate, entityName);

    public static string Restored(string entityName) =>
        string.Format(RestoredTemplate, entityName);

    public const string Retrieved =
        "Data retrieved successfully.";

    public const string LoginSuccess =
        "Login successful.";

    public const string OtpSent =
        "OTP sent successfully.";

    public const string OtpVerified =
        "OTP verified successfully.";
}
