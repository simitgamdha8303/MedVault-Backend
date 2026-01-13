namespace MedVault.Models.Dtos.ResponseDtos;


public class DoctorListResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string DoctorName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public int HospitalId { get; set; }
    public string HospitalName { get; set; } = default!;
    public string Specialization { get; set; } = default!;
    public string LicenseNumber { get; set; } = default!;
    public bool IsVarified { get; set; }
    public DateTime CreatedAt { get; set; }
}
