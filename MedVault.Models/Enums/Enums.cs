namespace MedVault.Models.Enums;

public enum Gender
{
    Male,
    Female,
    Other
}

public enum BloodGroup
{
    A_Positive,
    A_Negative,
    B_Positive,
    B_Negative,
    AB_Positive,
    AB_Negative,
    O_Positive,
    O_Negative
}

public enum CheckupType
{
    Routine,
    Emergency,
    FollowUp,
    Surgery,
    Diagnostic
}

public enum Role
{
    Admin,
    Doctor,
    Patient
}

public enum DocumentType
{
    Jpg,
    Png,
    Pdf

}

public enum RecurrenceType
{
    None = 0,
    Daily = 1,
    Weekly = 2
}

public enum AppointmentStatus
{
    Pending,
    Confirmed,
    Completed,
    Cancelled,
}

