using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Services.IServices;
using Microsoft.AspNetCore.Mvc;
namespace MedVault.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserRequest userRequest)
    {
        Response<string> response = await _userService.RegisterUserAsync(userRequest);
        return Ok(response);
    }

}
