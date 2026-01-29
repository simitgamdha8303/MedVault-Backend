namespace MedVault.Models.Dtos.ResponseDtos;

using MedVault.Models.Enums;

public class AppointmentResponse
{
    public int Id { get; set; }

    public string? PatientName { get; set; }

    public string? DoctorName { get; set; }

    public DateTime AppointmentDate { get; set; }

    public TimeSpan AppointmentTime { get; set; }

    public CheckupType CheckupType { get; set; }

    public string CheckupTypeValue => CheckupType.ToString();

    public AppointmentStatus Status { get; set; }

    public string StatusValue => Status.ToString();

    public DateTime CreatedAt { get; set; }
}
