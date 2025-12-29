using MedVault.Models.Enums;

namespace MedVault.Models.Dtos.ResponseDtos;

public class UserResponse
{
    public Role Role { get; set; }
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string MiddleName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Mobile { get; set; } = null!;
}
