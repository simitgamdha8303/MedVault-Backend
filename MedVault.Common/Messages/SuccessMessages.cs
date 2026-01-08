namespace MedVault.Common.Messages;

public static class SuccessMessages
{

    private const string REGISTERED_TEMPLATE = "{0} registered successfully.";
    private const string UPDATED_TEMPLATE = "{0} updated successfully.";
    private const string DELETED_TEMPLATE = "{0} deleted successfully.";
    private const string RESTORED_TEMPLATE = "{0} restored successfully.";
    private const string PROFILE_CREATED = "{0} profile created successfully.";
    private const string CREATED = "{0} created successfully.";
    private const string PROFILE_RETRIEVED = "{0} profile retrieved successfully.";

    public static string Registered(string entityName) =>
        string.Format(REGISTERED_TEMPLATE, entityName);

    public static string Updated(string entityName) =>
        string.Format(UPDATED_TEMPLATE, entityName);

    public static string Deleted(string entityName) =>
        string.Format(DELETED_TEMPLATE, entityName);

    public static string Restored(string entityName) =>
        string.Format(RESTORED_TEMPLATE, entityName);

    public static string ProfileCreated(string entityName) =>
        string.Format(PROFILE_CREATED, entityName);

    public static string Created(string entityName) =>
        string.Format(CREATED, entityName);

    public static string ProfileRetrieved(string entityName) =>
        string.Format(PROFILE_RETRIEVED, entityName);

    public const string RETRIEVED =
        "Data retrieved successfully.";

    public const string LOGIN_SUCCESS =
        "Login successful.";

    public const string OTP_SENT =
        "OTP sent successfully.";

    public const string OTP_VERIFIED =
        "OTP verified successfully.";

}
