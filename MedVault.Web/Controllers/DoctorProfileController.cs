using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedVault.Web.Controllers;

[ApiController]
[Route("api/doctor-profiles")]
[Authorize(Roles = "Doctor")]
public class DoctorProfileController(IDoctorProfileService doctorProfileService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(DoctorProfileRequest doctorProfileRequest)
    {
        Response<string> createDoctorProfileResponse = await doctorProfileService.CreateAsync(doctorProfileRequest);
        return Ok(createDoctorProfileResponse);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        Response<DoctorProfileResponse> getDoctorProfileResponse = await doctorProfileService.GetByIdAsync(id);
        return Ok(getDoctorProfileResponse);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, DoctorProfileRequest doctorProfileRequest)
    {
        Response<string> updateDoctorProfileResponse = await doctorProfileService.UpdateAsync(id, doctorProfileRequest);
        return Ok(updateDoctorProfileResponse);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        Response<string> deleteDoctorProfileResponse = await doctorProfileService.DeleteAsync(id);
        return Ok(deleteDoctorProfileResponse);
    }
}
