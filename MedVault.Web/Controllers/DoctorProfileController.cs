using System.Security.Claims;
using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedVault.Web.Controllers;

[ApiController]
[Route("api/doctor-profile")]
// [Authorize(Roles = "Doctor")]
public class DoctorProfileController(IDoctorProfileService doctorProfileService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(DoctorProfileRequest doctorProfileRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        int userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        Response<string> createDoctorProfileResponse = await doctorProfileService.CreateAsync(doctorProfileRequest, userId);
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
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Response<string> updateDoctorProfileResponse = await doctorProfileService.UpdateAsync(id, doctorProfileRequest);
        return Ok(updateDoctorProfileResponse);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        Response<string> deleteDoctorProfileResponse = await doctorProfileService.DeleteAsync(id);
        return Ok(deleteDoctorProfileResponse);
    }

    [HttpGet]
    // [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAll()
    {
        Response<List<DoctorListResponse>>? doctorListResponse = await doctorProfileService.GetAllAsync();
        return Ok(doctorListResponse);
    }

    [HttpGet("hospitals")]
    // [Authorize(Roles = "Doctor,Admin")]
    public async Task<IActionResult> GetHospitals()
    {
         Response<List<HospitalResponse>>? hospitalListResponse = await doctorProfileService.GetAllHospitalByFnAsync();
        return Ok(hospitalListResponse);
    }



}
