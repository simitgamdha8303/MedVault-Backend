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
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        Response<LoginResponse> response = await _authService.LoginUserAsync(loginRequest);
        return Ok(response);
    }

    [HttpPost("verify-otp")]
    public async Task<IActionResult> VerifyOtp(VerifyOtpRequest request)
    {
        Response<string> response = await _authService.VerifyOtpAsync(request);
        return Ok(response);
    }

    [HttpPost("resend-otp")]
    public async Task<IActionResult> ResendOtp(ResendOtpRequest request)
    {
        Response<string> response = await _authService.ResendOtpAsync(request);
        return Ok(response);
    }


}
