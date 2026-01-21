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
[Route("api/patient-profile")]
public class PatientProfileController(IPatientProfileService patientProfileService)
    : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> Create([FromBody] PatientProfileRequest patientProfileRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        if (userId == 0)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        Response<string> createPatientProfileResponse = await patientProfileService.CreateAsync(patientProfileRequest, userId);
        return Ok(createPatientProfileResponse);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Doctor,Patient")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        Response<PatientProfileResponse> patientProfileResponse = await patientProfileService.GetByIdAsync(id);
        return Ok(patientProfileResponse);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> Update(int id, PatientProfileRequest patientProfileRequest)
    {
        if (id <= 0)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Response<string> updatePatientProfileResponse = await patientProfileService.UpdateAsync(id, patientProfileRequest);
        return Ok(updatePatientProfileResponse);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Patient")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id <= 0)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        Response<string> deletePatientProfile = await patientProfileService.DeleteAsync(id);
        return Ok(deletePatientProfile);
    }
}
