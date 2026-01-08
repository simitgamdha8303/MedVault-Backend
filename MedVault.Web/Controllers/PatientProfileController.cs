using System.Security.Claims;
using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedVault.Web.Controllers;

[ApiController]
[Route("api/patient-profile")]
[Authorize(Roles = "Patient")]
public class PatientProfileController(IPatientProfileService patientProfileService)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PatientProfileRequest patientProfileRequest)
    {
        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Response<string> createPatientProfileResponse = await patientProfileService.CreateAsync(patientProfileRequest, userId);
        return Ok(createPatientProfileResponse);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        Response<PatientProfileResponse> patientProfileResponse = await patientProfileService.GetByIdAsync(id);
        return Ok(patientProfileResponse);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, PatientProfileRequest patientProfileRequest)
    {
        Response<string> updatePatientProfileResponse = await patientProfileService.UpdateAsync(id, patientProfileRequest);
        return Ok(updatePatientProfileResponse);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        Response<string> deletePatientProfile = await patientProfileService.DeleteAsync(id);
        return Ok(deletePatientProfile);
    }
}
