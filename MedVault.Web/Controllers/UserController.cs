using System.Security.Claims;
using MedVault.Common.Messages;
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

    private int GetUserId()
    {
        string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        return userId;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRequest userRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Response<string> registerUserResponse = await userService.RegisterUserAsync(userRequest);
        return Ok(registerUserResponse);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyProfile()
    {
        Response<UserProfileResponse>? userProfile = await userService.GetMyProfileAsync(GetUserId());
        return Ok(userProfile);
    }

    [HttpPut("two-factor")]
    [Authorize]
    public async Task<IActionResult> UpdateTwoFactor(
    ToggleTwoFactorRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        Response<bool> twoFactorResponse =
            await userService.UpdateTwoFactorAsync(GetUserId(), request.Enabled);

        return Ok(twoFactorResponse);
    }

    [HttpPut("profile")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile(UpdateUserProfileRequest updateUserProfileRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Response<bool> updateProfileResponse = await userService.UpdateProfileAsync(GetUserId(), updateUserProfileRequest);

        return Ok(updateProfileResponse);
    }

}
