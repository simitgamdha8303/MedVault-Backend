namespace MedVault.Models.Dtos.ResponseDtos;

public class PatientQrAccessResponse
{
    public int PatientId { get; set; }

    public PatientProfileResponse PatientProfile { get; set; } = null!;

    public List<MedicalTimelineResponse> MedicalTimelines { get; set; } = new();
}
