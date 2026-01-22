namespace MedVault.Models.Dtos.ResponseDtos;

public class DoctorVisitChartPointResponse
{
    public string Label { get; set; } = null!;
    public int Count { get; set; }
    public List<string> Patients { get; set; } = new();
}
