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
[Authorize]
public class ReminderController(IReminderService reminderService)
    : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> Create(CreateReminderRequest createReminderRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        Response<int> createReminderResponse = await reminderService.CreateAsync(createReminderRequest, userId);

        return Ok(createReminderResponse);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        Response<ReminderResponse> getByIdReminderResponse = await reminderService.GetByIdAsync(id);

        return Ok(getByIdReminderResponse);
    }

    [HttpGet("patient")]
    public async Task<IActionResult> GetByPatient()
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        Response<List<ReminderResponse>> getByPatientRemindersResponse = await reminderService.GetByPatientAsync(userId);

        return Ok(getByPatientRemindersResponse);
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

        return Ok(updateReminderResponse);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        Response<string> deleteReminderResponse = await reminderService.DeleteAsync(id);

        return Ok(deleteReminderResponse);
    }
}
