using System.Security.Claims;
using MedVault.Common.Messages;
using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedVault.Web.Controllers;

[ApiController]
[Route("api/qr-share")]
[Authorize]
public class QrShareController(IQrShareService qrShareService) : ControllerBase
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

    [HttpPost]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> Generate([FromBody] GenerateQrRequest generateQrRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Response<string> generateQrResponse = await qrShareService.GenerateAsync(GetUserId(), generateQrRequest);
        return StatusCode(generateQrResponse.StatusCode, generateQrResponse);
    }

    [HttpGet("patient")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetByPatient()
    {
        Response<List<QrShareResponse>> getByPatientQrShareResponse = await qrShareService.GetByPatientAsync(GetUserId());

        return StatusCode(getByPatientQrShareResponse.StatusCode, getByPatientQrShareResponse);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetById(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        Response<QrShareResponse> getByIdReminderResponse = await qrShareService.GetByIdAsync(id);

        return StatusCode(getByIdReminderResponse.StatusCode, getByIdReminderResponse);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        Response<string> deleteQrShareResponse = await qrShareService.DeleteAsync(id);

        return StatusCode(deleteQrShareResponse.StatusCode, deleteQrShareResponse);
    }

    [HttpGet("doctor")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetByDoctor()
    {
        Response<List<QrShareByDoctorResponse>> getByDoctorQrShareResponse = await qrShareService.GetByDoctorAsync(GetUserId());

        return StatusCode(getByDoctorQrShareResponse.StatusCode, getByDoctorQrShareResponse);
    }

}