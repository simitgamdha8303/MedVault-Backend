using System.Security.Claims;
using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedVault.Web.Controllers;

[ApiController]
[Route("api/qr-share")]
[Authorize(Roles = "Patient")]
public class QrShareController(IQrShareService qrShareService) : ControllerBase
{
    [HttpPost("generate")]
    public async Task<IActionResult> Generate([FromBody] GenerateQrRequestDto request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        Response<GenerateQrResponseDto> generateQrResponse = await qrShareService.GenerateAsync(userId, request);
        return StatusCode(generateQrResponse.StatusCode, generateQrResponse);
    }
}