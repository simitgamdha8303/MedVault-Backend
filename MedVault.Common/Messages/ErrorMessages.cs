namespace MedVault.Common.Messages;

public static class ErrorMessages
{
    // public static string NotFound(string entityName) => $"{entityName} not found.";
    // public static string AlreadyExists(string entityName) => $"{entityName} already exists.";
    // public static string Invalid(string entityName) => $"{entityName} is invalid.";
    // public const string Unauthorized = "You are not authorized to perform this action.";
    // public const string InternalError = "An unexpected error occurred.";
    // public const string InvalidEmailOrPhone = "Invalid email or phone number";

    private const string NotFoundTemplate = "{0} not found.";
    private const string AlreadyExistsTemplate = "{0} already exists.";
    private const string InvalidTemplate = "{0} is invalid.";

    public static string NotFound(string entityName) =>
        string.Format(NotFoundTemplate, entityName);

    public static string AlreadyExists(string entityName) =>
        string.Format(AlreadyExistsTemplate, entityName);

    public static string Invalid(string entityName) =>
        string.Format(InvalidTemplate, entityName);

    public const string Unauthorized =
        "You are not authorized to perform this action.";

    public const string InternalError =
        "An unexpected error occurred.";

    public const string InvalidEmailOrPhone =
        "Invalid email or phone number";
}
