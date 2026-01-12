using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;

namespace MedVault.Services.IServices;

public interface IUserService
{
    Task<Response<string>> RegisterUserAsync(UserRequest userRequest);
    Task<Response<UserProfileResponse>> GetMyProfileAsync(int userId);
    Task<Response<bool>> UpdateTwoFactorAsync(int userId, bool enabled);
    Task<Response<bool>> UpdateProfileAsync(int userId, UpdateUserProfileRequest updateUserProfileRequest);

}
