using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace MedVault.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
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

}
