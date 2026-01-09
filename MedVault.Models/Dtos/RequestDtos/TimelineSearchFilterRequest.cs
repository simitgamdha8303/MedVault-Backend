using MedVault.Models.Enums;

namespace MedVault.Models.Dtos.RequestDtos;

public class TimelineSearchFilterRequest
{

    public CheckupType? CheckupType { get; set; }

    public int? DoctorProfileId { get; set; }

    public DateOnly? FromDate { get; set; }

    public DateOnly? ToDate { get; set; }
}
