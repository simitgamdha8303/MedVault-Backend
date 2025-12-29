namespace MedVault.Common.Messages;

public static class SuccessMessages
{
    public static string Registered(string entityName) => $"{entityName} registered successfully.";
    public static string Updated(string entityName) => $"{entityName} updated successfully.";
    public static string Deleted(string entityName) => $"{entityName} deleted successfully.";
    public static string Restored(string entityName) => $"{entityName} restored successfully.";
    public const string Retrieved = "Data retrieved successfully.";
}
