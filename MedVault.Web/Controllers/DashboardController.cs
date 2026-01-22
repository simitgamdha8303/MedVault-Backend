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
    [HttpGet("total-records")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetMedicalTimelineCount()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (userId == 0)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        Response<int> medicalTimelineCount = await dashboardService.GetMedicalTimelineCount(userId);
        return Ok(medicalTimelineCount);
    }

    [HttpGet("last-visit")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetPatientLastVisit()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (userId == 0)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        Response<PatientLastVisitResponse> lastVisit = await dashboardService.GetLastVisit(userId);
        return Ok(lastVisit);
    }

    [HttpGet("upcoming-appointment")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetUpcomingAppointment()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (userId == 0)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        Response<string> upcomingAppointment = await dashboardService.GetUpcomingAppointment(userId);
        return Ok(upcomingAppointment);
    }

    [HttpGet("visit-chart")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> GetVisitChart([FromQuery] string filter)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (userId == 0)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }
        return Ok(await dashboardService.GetVisitChart(userId, filter));
    }

    [HttpGet("last-checkup")]
    public async Task<IActionResult> GetLastCheckup()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (userId == 0)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }
        return Ok(await dashboardService.GetLastPatientCheckup(userId));
    }

    [HttpGet("total-checkups")]
    public async Task<IActionResult> GetTotalCheckups()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (userId == 0)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }
        return Ok(await dashboardService.GetTotalPatientCheckups(userId));
    }

    [HttpGet("top-patients")]
    public async Task<IActionResult> GetTopPatients()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (userId == 0)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }
        return Ok(await dashboardService.GetTopPatients(userId));
    }

    [HttpGet("doctor-visit-chart")]
    public async Task<IActionResult> GetDoctorVisitChart([FromQuery] string filter)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (userId == 0)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }
        return Ok(await dashboardService.GetPatientVisitChart(userId, filter));
    }

}
