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
[Route("api/reminder")]
[Authorize(Roles = "Patient")]
public class ReminderController(IReminderService reminderService)
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
    public async Task<IActionResult> Create([FromBody] CreateReminderRequest createReminderRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Response<int> createReminderResponse = await reminderService.CreateAsync(createReminderRequest, GetUserId());

        return StatusCode(createReminderResponse.StatusCode, createReminderResponse);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        Response<ReminderResponse> getByIdReminderResponse = await reminderService.GetByIdAsync(id);

        return StatusCode(getByIdReminderResponse.StatusCode, getByIdReminderResponse);
    }

    [HttpGet("patient")]
    public async Task<IActionResult> GetByPatient()
    {

        Response<List<ReminderResponse>> getByPatientRemindersResponse = await reminderService.GetByPatientAsync(GetUserId());

        return StatusCode(getByPatientRemindersResponse.StatusCode, getByPatientRemindersResponse);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, UpdateReminderRequest updateReminderRequest)
    {
        if (id <= 0)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Response<int> updateReminderResponse = await reminderService.UpdateAsync(id, updateReminderRequest);

        return StatusCode(updateReminderResponse.StatusCode, updateReminderResponse);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        Response<string> deleteReminderResponse = await reminderService.DeleteAsync(id);

        return StatusCode(deleteReminderResponse.StatusCode, deleteReminderResponse);
    }
}
