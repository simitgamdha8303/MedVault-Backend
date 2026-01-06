using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;

namespace MedVault.Services.IServices;

public interface IUserService
{
    Task<Response<string>> RegisterUserAsync(UserRequest userRequest);
}
