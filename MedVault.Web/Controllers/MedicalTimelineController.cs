using System.Security.Claims;
using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Enums;
using MedVault.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedVault.Web.Controllers;

[ApiController]
[Route("api/medical-timeline")]
[Authorize]
public class MedicalTimelineController(IMedicalTimelineService medicalTimelineService)
    : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> Create([FromBody] MedicalTimelineRequest medicalTimelineRequest)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Response<string> timelineCreateResponse = await medicalTimelineService.CreateAsync(medicalTimelineRequest, userId);
        return Ok(timelineCreateResponse);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        Response<MedicalTimelineResponse> timelineByIdResponse = await medicalTimelineService.GetByIdAsync(id);
        return Ok(timelineByIdResponse);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, MedicalTimelineRequest medicalTimelineRequest)
    {
        Response<string> timelineUpdateResponse = await medicalTimelineService.UpdateAsync(id, medicalTimelineRequest);
        return Ok(timelineUpdateResponse);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        Response<string> timelineDeleteResponse = await medicalTimelineService.DeleteAsync(id);
        return Ok(timelineDeleteResponse);
    }

    [HttpPost("patient")]
    public async Task<IActionResult> GetByPatientId([FromBody] TimelineSearchFilterRequest searchRequest)
    {
         int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Response<List<MedicalTimelineResponse>> timelineByPatientIdResponse = await medicalTimelineService.GetFilteredAsync(userId, searchRequest);
        return Ok(timelineByPatientIdResponse);
    }

    [HttpGet("checkup-types")]
    public IActionResult GetCheckupTypes()
    {
        var values = Enum.GetValues(typeof(CheckupType))
            .Cast<CheckupType>()
            .Select(e => new
            {
                id = (int)e,
                name = e.ToString()
            });

        return Ok(values);
    }
}
