using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Services.IServices;
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

}
