namespace MedVault.Models.Dtos.ResponseDtos;

public class TopPatientResponse
{
    public string PatientName { get; set; } = null!;
    public int VisitCount { get; set; }
}
