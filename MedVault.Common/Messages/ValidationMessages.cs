namespace MedVault.Common.Messages;

public static class ValidationMessages
{
    
    public const string EmailRequired = "Email is required.";
    public const string InvalidEmail = "Invalid email address.";
    public const string EmailSpecialChar = "Email cannot start with a special character.";
    public const string FirstNameRequired = "FirstName is required.";
    public const string FirstNameOnlyLetters = "FirstName must contain only letters and cannot be just spaces.";
    public const string MiddleNameRequired = "MiddleName is required.";
    public const string MiddleNameOnlyLetters = "MiddleName must contain only letters and cannot be just spaces.";
    public const string LastNameRequired = "LastName is required.";
    public const string LastNameOnlyLetters = "LastName must contain only letters and cannot be just spaces.";
    public const string MobileRequired = "Mobile is required.";
    public const string ContactnumberFormat = "Contact Number must be exactly 10 digits and cannot start with 0.";
    public const string PasswordRequired = "Password is required.";
    public const string PasswordMinLength = "Password must be at least 8 characters long.";
    public const string PasswordRequirements = "Password must be at least 8 characters, include uppercase, lowercase, number, special character, and have no spaces.";
}
