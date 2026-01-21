namespace MedVault.Common.Messages;

public static class ErrorMessages
{

    private const string NOT_FOUND_TEMPLATE = "{0} not found.";
    private const string ALREDY_EXISTS_TEMPLATE = "{0} already exists.";
    private const string INVALID_TEMPLATE = "{0} is invalid.";

    public static string NotFound(string entityName) =>
        string.Format(NOT_FOUND_TEMPLATE, entityName);

    public static string AlreadyExists(string entityName) =>
        string.Format(ALREDY_EXISTS_TEMPLATE, entityName);

    public static string Invalid(string entityName) =>
        string.Format(INVALID_TEMPLATE, entityName);

    public const string UNAUTHORIZED =
        "You are not authorized to perform this action.";

    public const string INTERNAL_ERROR =
        "An unexpected error occurred.";

    public const string INVALID_EMAIL_OR_PHONE =
        "Invalid email or phone number";

    public const string DOCTOR_PROFILE_NOT_SELECTED =
        "Doctor name is required when doctor profile is not selected.";

    public const string FILE_VALIDATION =
        "Only JPG, PNG and PDF files are allowed";

    public const string END_DATE_BEFORE =
        "Recurrence end date must be after reminder time";
}
