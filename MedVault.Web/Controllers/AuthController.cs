using CloudinaryDotNet;
using MedVault.Common.Response;
using MedVault.Models;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MedVault.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, Cloudinary cloudinary,  IOptions<CloudinarySettingsResponse> cloudinaryOptions) : ControllerBase
{

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        Response<LoginResponse> loginUserResponse = await authService.LoginUserAsync(loginRequest);
        return Ok(loginUserResponse);
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
    {
        Response<OtpResponse> verifyOtpResponse = await authService.VerifyOtpAsync(request);
        return Ok(verifyOtpResponse);
    }

    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp(ResendOtpRequest request)
    {
        Response<string> resendOtpResponse = await authService.ResendOtpAsync(request);
        return Ok(resendOtpResponse);
    }

    [HttpPost("signature")]
    public IActionResult GetSignature()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        var parameters = new SortedDictionary<string, object>
        {
            { "timestamp", timestamp },
            { "folder", "medvault/documents" }
        };

        string signature = cloudinary.Api.SignParameters(parameters);

        return Ok(new
        {
            timestamp,
            signature,
            cloudName = cloudinaryOptions.Value.CloudName,
            apiKey = cloudinaryOptions.Value.ApiKey
        });
    }

}
