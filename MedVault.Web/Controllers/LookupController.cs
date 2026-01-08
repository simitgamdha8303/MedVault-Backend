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
        return Ok(doctorList);
    }

    [HttpGet("checkup-types")]
    public IActionResult GetCheckupTypes()
    {
        Response<List<EnumLookupResponse>> checkupTypes =  lookupService.GetCheckupTypes();
        return Ok(checkupTypes);
    }

    [HttpGet("genders")]
    public IActionResult GetGenders()
    {
        Response<List<EnumLookupResponse>> genders = lookupService.GetGenders();
        return Ok(genders);
    }

    [HttpGet("hospitals")]
    public async Task<IActionResult> GetAllHospital()
    {
        Response<List<EnumLookupResponse>> hospitalList = await lookupService.GetAllHospitalAsync();
        return Ok(hospitalList);
    }

    [HttpGet("blood-groups")]
    public IActionResult GetBloodGroups()
    {
        Response<List<EnumLookupResponse>> bloodGroups = lookupService.GetBloodGroups();
        return Ok(bloodGroups);
    }
}