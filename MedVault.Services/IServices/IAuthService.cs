using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;

namespace MedVault.Services.IServices;

public interface IAuthService
{
    Task<Response<LoginResponse>> LoginUserAsync(LoginRequest loginRequest);
    Task<Response<OtpResponse>> VerifyOtpAsync(VerifyOtpRequest request);
    Task<Response<string>> ResendOtpAsync(ResendOtpRequest request);


}
