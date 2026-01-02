namespace MedVault.Common.Messages;

public static class ValidationMessages
{

    public const string EMAIL_REQUIRED = "Email is required.";
    public const string INVALID_EMAIL = "Invalid email address.";
    public const string EMAIL_SPECIAL_CHAR = "Email cannot start with a special character.";
    public const string FIRST_NAME_REQUIRED = "FirstName is required.";
    public const string FIRST_NAME_ONLY_LETTERS = "FirstName must contain only letters and cannot be just spaces.";
    public const string LAST_NAME_REQUIRED = "LastName is required.";
    public const string LAST_NAME_ONLY_LETTERS = "LastName must contain only letters and cannot be just spaces.";
    public const string MOBILE_REQUIRED = "Mobile is required.";
    public const string CONTACT_NUMBER_FORMAT = "Contact Number must be exactly 10 digits and cannot start with 0.";
    public const string PASSWORD_REQUIRED = "Password is required.";
    public const string PASSWORD_MIN_LENGTH = "Password must be at least 8 characters long.";
    public const string PASSWORD_REQUIREMENTS = "Password must be at least 8 characters, include uppercase, lowercase, number, special character, and have no spaces.";
    public const string IDENTIFIER_REQUIRED = "Email or phone number is required";
}
