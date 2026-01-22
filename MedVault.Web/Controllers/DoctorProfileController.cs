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
[Route("api/doctor-profile")]
[Authorize(Roles = "Doctor")]
public class DoctorProfileController(IDoctorProfileService doctorProfileService) : ControllerBase
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
    public async Task<IActionResult> Create(DoctorProfileRequest doctorProfileRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Response<string> createDoctorProfileResponse = await doctorProfileService.CreateAsync(doctorProfileRequest, GetUserId());
        return Ok(createDoctorProfileResponse);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id <= 0)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        Response<DoctorProfileResponse> getDoctorProfileResponse = await doctorProfileService.GetByIdAsync(id);
        return Ok(getDoctorProfileResponse);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, DoctorProfileRequest doctorProfileRequest)
    {
        if (id <= 0)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

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
        if (id <= 0)
        {
            return BadRequest(ErrorMessages.Invalid("id"));
        }

        Response<string> deleteDoctorProfileResponse = await doctorProfileService.DeleteAsync(id);
        return Ok(deleteDoctorProfileResponse);
    }

    [HttpGet("patients")]
    public async Task<IActionResult> GetPatientsByDoctor()
    {

        Response<List<DoctorPatientListResponse>> response =
            await doctorProfileService.GetPatientsByDoctorIdAsync(GetUserId());

        return Ok(response);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Response<List<DoctorListResponse>>? doctorListResponse = await doctorProfileService.GetAllAsync();
        return Ok(doctorListResponse);
    }

    [HttpGet("hospitals-by-fn")]
    public async Task<IActionResult> GetHospitals()
    {
        Response<List<HospitalResponse>>? hospitalListResponse = await doctorProfileService.GetAllHospitalByFnAsync();
        return Ok(hospitalListResponse);
    }

    [HttpPost("add-hospital-by-sp")]
    public async Task<IActionResult> AddHospital(HospitalCreateRequest hospitalCreateRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Response<int> addHospitalResponse = await doctorProfileService.AddHospitalBySp(hospitalCreateRequest);

        return Ok(addHospitalResponse);
    }

    [HttpPost("add-doctor-by-sp")]
    public async Task<IActionResult> AddDoctorProfileBySp(DoctorProfileRequest doctorProfileRequest)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        Response<DoctorProfileResponse> doctorProfileResponse = await doctorProfileService.AddDoctorProfileBySp(doctorProfileRequest, GetUserId());
        return Ok(doctorProfileResponse);
    }
}
