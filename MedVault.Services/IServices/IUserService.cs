using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;

namespace MedVault.Services.IServices;

public interface IUserService
{
    Task<Response<UserResponse?>> RegisterUserAsync(UserRequest userRequest);
}
