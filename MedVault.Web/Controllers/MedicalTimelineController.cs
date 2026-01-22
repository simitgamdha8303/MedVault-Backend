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
    public async Task<IActionResult> Create([FromBody] MedicalTimelineRequest medicalTimelineRequest)
    {

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Response<int> timelineCreateResponse = await medicalTimelineService.CreateAsync(medicalTimelineRequest, GetUserId());
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

        Response<List<MedicalTimelineResponse>> timelineByPatientIdResponse = await medicalTimelineService.GetFilteredAsync(GetUserId(), searchRequest);
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

        Response<int> addDocumentResponse = await medicalTimelineService.AddDocumentAsync(documentRequest, GetUserId());
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

        Response<string> deleteManyResponse =
            await medicalTimelineService.DeleteManyDocumentAsync(request.DocumentIds, GetUserId());

        return Ok(deleteManyResponse);
    }
}
