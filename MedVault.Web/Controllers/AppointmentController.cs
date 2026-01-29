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
[Authorize]
public class AppointmentController(IAppointmentService appointmentService) : ControllerBase
{
    private int GetUserId()
    {
        string? userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (string.IsNullOrWhiteSpace(userIdClaim) ||
            !int.TryParse(userIdClaim, out int userId))
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        return userId;
    }

    [HttpPost]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> Book([FromBody] BookAppointmentRequest bookAppointmentRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Response<string> bookAppointmentResponse = await appointmentService.BookAsync(GetUserId(), bookAppointmentRequest);

        return StatusCode(bookAppointmentResponse.StatusCode, bookAppointmentResponse);
    }

    [HttpGet("patient")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetByPatient()
    {
        Response<List<AppointmentResponse>> response =
            await appointmentService.GetByPatientAsync(GetUserId());

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("doctor/pending")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetPendingForDoctor()
    {
        Response<List<AppointmentResponse>> response =
            await appointmentService.GetByDoctorAsync(GetUserId());

        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id}/approve")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Approve(int id)
    {
        Response<string> response =
            await appointmentService.ApproveAsync(id, GetUserId());

        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("{id}/reject")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> Reject(int id)
    {
        Response<string> response =
            await appointmentService.RejectAsync(id, GetUserId());

        return StatusCode(response.StatusCode, response);
    }
}