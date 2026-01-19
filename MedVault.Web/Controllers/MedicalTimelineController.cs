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
[Route("api/medical-timeline")]
[Authorize]
public class MedicalTimelineController(IMedicalTimelineService medicalTimelineService)
    : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> Create([FromBody] MedicalTimelineRequest medicalTimelineRequest)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Response<int> timelineCreateResponse = await medicalTimelineService.CreateAsync(medicalTimelineRequest, userId);
        return Ok(timelineCreateResponse);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        Response<MedicalTimelineResponse> timelineByIdResponse = await medicalTimelineService.GetByIdAsync(id);
        return Ok(timelineByIdResponse);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, MedicalTimelineRequest medicalTimelineRequest)
    {
        if (id <= 0)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Response<int> timelineUpdateResponse = await medicalTimelineService.UpdateAsync(id, medicalTimelineRequest);
        return Ok(timelineUpdateResponse);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        Response<string> timelineDeleteResponse = await medicalTimelineService.DeleteAsync(id);
        return Ok(timelineDeleteResponse);
    }

    [HttpPost("patient")]
    public async Task<IActionResult> GetByPatientId([FromBody] TimelineSearchFilterRequest searchRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Response<List<MedicalTimelineResponse>> timelineByPatientIdResponse = await medicalTimelineService.GetFilteredAsync(userId, searchRequest);
        return Ok(timelineByPatientIdResponse);
    }

    [HttpPost("patient-document")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> Create([FromBody] DocumentRequest documentRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        Response<int> addDocumentResponse = await medicalTimelineService.AddDocumentAsync(documentRequest, userId);
        return Ok(addDocumentResponse);
    }

    [HttpDelete("documents/list")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> DeleteMany([FromBody] DeleteDocumentsRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        Response<string> deleteManyResponse =
            await medicalTimelineService.DeleteManyDocumentAsync(request.DocumentIds, userId);

        return Ok(deleteManyResponse);
    }
}
