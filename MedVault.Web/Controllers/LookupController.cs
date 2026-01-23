using MedVault.Common.Response;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedVault.Web.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class LookupController(ILookupService lookupService) : ControllerBase
{
    [HttpGet("doctors")]
    public async Task<IActionResult> GetAllDoctor()
    {
        Response<List<EnumLookupResponse>> doctorList = await lookupService.GetAllDoctorAsync();
        return StatusCode(doctorList.StatusCode, doctorList);
    }

    [HttpGet("checkup-types")]
    public IActionResult GetCheckupTypes()
    {
        Response<List<EnumLookupResponse>> checkupTypes = lookupService.GetCheckupTypes();
        return StatusCode(checkupTypes.StatusCode, checkupTypes);
    }

    [HttpGet("genders")]
    public IActionResult GetGenders()
    {
        Response<List<EnumLookupResponse>> genders = lookupService.GetGenders();
        return StatusCode(genders.StatusCode, genders);
    }

    [HttpGet("hospitals")]
    public async Task<IActionResult> GetAllHospital()
    {
        Response<List<EnumLookupResponse>> hospitalList = await lookupService.GetAllHospitalAsync();
        return StatusCode(hospitalList.StatusCode, hospitalList);
    }

    [HttpGet("blood-groups")]
    public IActionResult GetBloodGroups()
    {
        Response<List<EnumLookupResponse>> bloodGroups = lookupService.GetBloodGroups();
        return StatusCode(bloodGroups.StatusCode, bloodGroups);
    }

    [HttpGet("recurrence-type")]
    public IActionResult GetRecurrenceType()
    {
        Response<List<EnumLookupResponse>> recurrenceTypes = lookupService.GetRecurrenceType();
        return StatusCode(recurrenceTypes.StatusCode, recurrenceTypes);
    }

    [HttpGet("reminder-type")]
    public async Task<IActionResult> GetAllReminderType()
    {
        Response<List<EnumLookupResponse>> reminderTypes = await lookupService.GetAllReminderTypeAsync();
        return StatusCode(reminderTypes.StatusCode, reminderTypes);
    }
}