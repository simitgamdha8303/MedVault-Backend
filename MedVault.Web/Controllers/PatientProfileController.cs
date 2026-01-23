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
    public async Task<IActionResult> Create([FromBody] PatientProfileRequest patientProfileRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Response<string> createPatientProfileResponse = await patientProfileService.CreateAsync(patientProfileRequest, GetUserId());
        return StatusCode(createPatientProfileResponse.StatusCode, createPatientProfileResponse);
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
        return StatusCode(patientProfileResponse.StatusCode, patientProfileResponse);
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
        return StatusCode(updatePatientProfileResponse.StatusCode, updatePatientProfileResponse);
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
        return StatusCode(deletePatientProfile.StatusCode, deletePatientProfile);
    }
}
