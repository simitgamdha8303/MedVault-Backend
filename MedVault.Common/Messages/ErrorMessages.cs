namespace MedVault.Common.Messages;

public static class ErrorMessages
{
    public static string NotFound(string entityName) => $"{entityName} not found.";
    public static string AlreadyExists(string entityName) => $"{entityName} already exists.";
    public const string InvalidPassword = "Password and Confirm Password do not match.";
    public const string Unauthorized = "You are not authorized to perform this action.";
    public const string InternalError = "An unexpected error occurred.";
}
