namespace MedVault.Models.Dtos.ResponseDtos;

public class VisitChartPointResponse
{
    public string Label { get; set; } = null!;
    public int Count { get; set; }
    public List<string> Doctors { get; set; } = new();
}