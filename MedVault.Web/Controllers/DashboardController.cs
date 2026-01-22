using System.Security.Claims;
using MedVault.Common.Messages;
using MedVault.Common.Response;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedVault.Web.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController(IDashboardService dashboardService) : ControllerBase
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

    [HttpGet("total-records")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetMedicalTimelineCount()
    {
        Response<int> medicalTimelineCount = await dashboardService.GetMedicalTimelineCount(GetUserId());
        return Ok(medicalTimelineCount);
    }

    [HttpGet("last-visit")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetPatientLastVisit()
    {
        Response<PatientLastVisitResponse> lastVisit = await dashboardService.GetLastVisit(GetUserId());
        return Ok(lastVisit);
    }

    [HttpGet("upcoming-appointment")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetUpcomingAppointment()
    {
        Response<string> upcomingAppointment = await dashboardService.GetUpcomingAppointment(GetUserId());
        return Ok(upcomingAppointment);
    }

    [HttpGet("visit-chart")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetVisitChart([FromQuery] string filter)
    {

        return Ok(await dashboardService.GetVisitChart(GetUserId(), filter));
    }

    [HttpGet("last-checkup")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetLastCheckup()
    {
        return Ok(await dashboardService.GetLastPatientCheckup(GetUserId()));
    }

    [HttpGet("total-checkups")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetTotalCheckups()
    {
        return Ok(await dashboardService.GetTotalPatientCheckups(GetUserId()));
    }

    [HttpGet("top-patients")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetTopPatients()
    {
        return Ok(await dashboardService.GetTopPatients(GetUserId()));
    }

    [HttpGet("doctor-visit-chart")]
    [Authorize(Roles = "Doctor")]
    public async Task<IActionResult> GetDoctorVisitChart([FromQuery] string filter)
    {
        return Ok(await dashboardService.GetPatientVisitChart(GetUserId(), filter));
    }

}
