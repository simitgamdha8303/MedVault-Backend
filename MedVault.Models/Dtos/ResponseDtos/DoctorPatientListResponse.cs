namespace MedVault.Models.Dtos.ResponseDtos;

public class DoctorPatientListResponse
{
    public int PatientId { get; set; }
    public string PatientName { get; set; } = null!;
    public int TotalVisits { get; set; }
}
