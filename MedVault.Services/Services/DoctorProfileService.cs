using System.Net;
using System.Security.Claims;
using AutoMapper;
using MedVault.Common.Helper;
using MedVault.Common.Messages;
using MedVault.Common.Response;
using MedVault.Data.IRepositories;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;
using MedVault.Services.IServices;

namespace MedVault.Services.Services;

public class DoctorProfileService(
    IDoctorProfileRepository doctorProfileRepository,
    IUserRepository userRepository,
    IHospitalRepository hospitalRepository,
    IMapper mapper
) : IDoctorProfileService
{
    public async Task<Response<string>> CreateAsync(DoctorProfileRequest doctorProfileRequest,int userId)
    {

        // Validate User
        bool userExists = await userRepository.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        // Validate Hospital
        bool hospitalExists = await hospitalRepository.AnyAsync(h => h.Id == doctorProfileRequest.HospitalId);
        if (!hospitalExists)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Hospital"));
        }

        // Prevent duplicate 
        bool profileAlreadyExists = await doctorProfileRepository.AnyAsync(d => d.UserId == userId);

        if (profileAlreadyExists)
        {
            throw new ArgumentException(ErrorMessages.AlreadyExists("Doctor profile"));
        }

        DoctorProfile doctorProfile = mapper.Map<DoctorProfile>(doctorProfileRequest);
        doctorProfile.UserId = userId;
        doctorProfile.CreatedAt = DateTime.UtcNow;

        await doctorProfileRepository.AddAsync(doctorProfile);

        return ResponseHelper.Response<string>(
            data: null,
            succeeded: true,
            message: SuccessMessages.ProfileCreated("Doctor"),
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<DoctorProfileResponse>> GetByIdAsync(int id)
    {
        DoctorProfile? doctorProfile = await doctorProfileRepository.GetByIdAsync(id);

        if (doctorProfile == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Doctor profile"));
        }

        DoctorProfileResponse doctorProfileResponse = mapper.Map<DoctorProfileResponse>(doctorProfile);

        return ResponseHelper.Response(
            data: doctorProfileResponse,
            succeeded: true,
            message: SuccessMessages.ProfileRetrieved("Doctor"),
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string>> UpdateAsync(int id, DoctorProfileRequest doctorProfileRequest)
    {
        DoctorProfile? doctorProfile = await doctorProfileRepository.GetByIdAsync(id);

        if (doctorProfile == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Doctor profile"));
        }

        doctorProfile.HospitalId = doctorProfileRequest.HospitalId;
        doctorProfile.Specialization = doctorProfileRequest.Specialization;
        doctorProfile.LicenseNumber = doctorProfileRequest.LicenseNumber;
        doctorProfile.UpdatedAt = DateTime.UtcNow;

        doctorProfileRepository.Update(doctorProfile);
        await doctorProfileRepository.SaveChangesAsync();

        return ResponseHelper.Response<string>(
            data: null,
            succeeded: true,
            message: SuccessMessages.Updated("Doctor profile"),
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        DoctorProfile? doctorProfile = await doctorProfileRepository.GetByIdAsync(id);

        if (doctorProfile == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Doctor profile"));
        }

        doctorProfileRepository.Delete(doctorProfile);
        await doctorProfileRepository.SaveChangesAsync();

        return ResponseHelper.Response<string>(
            data: null,
            succeeded: true,
            message: SuccessMessages.Deleted("Doctor profile"),
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<List<HospitalResponse>>> GetAllHospitalAsync()
    {
        IEnumerable<Hospital>? hospitals = await hospitalRepository.GetAllAsync();

        if (hospitals == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Hospital"));
        }

        List<HospitalResponse>? hospitalResponses = mapper.Map<List<HospitalResponse>>(hospitals);

        return ResponseHelper.Response(
            data: hospitalResponses,
            succeeded: true,
            message: "Hospitals retrieved",
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }
}
