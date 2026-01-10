using System.Security.Claims;
using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace MedVault.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRequest userRequest)
    {
        Response<string> registerUserResponse = await userService.RegisterUserAsync(userRequest);
        return Ok(registerUserResponse);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyProfile()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        Response<UserProfileResponse>? userProfile = await userService.GetMyProfileAsync(userId);
        return Ok(userProfile);
    }

    [HttpPut("two-factor")]
    [Authorize]
    public async Task<IActionResult> UpdateTwoFactor(
    ToggleTwoFactorRequest request)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        Response<bool> twoFactorResponse =
            await userService.UpdateTwoFactorAsync(userId, request.Enabled);

        return Ok(twoFactorResponse);
    }


}
