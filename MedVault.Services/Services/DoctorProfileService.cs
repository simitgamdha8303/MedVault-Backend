using System.Net;
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
    public async Task<Response<string>> CreateAsync(DoctorProfileRequest doctorProfileRequest, int userId)
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

    public async Task<Response<List<DoctorListResponse>>> GetAllAsync()
    {
        List<DoctorListResponse>? doctors = await doctorProfileRepository.GetAllByFnAsync();

        return ResponseHelper.Response(
           data: doctors,
           succeeded: true,
           message: SuccessMessages.RETRIEVED,
           errors: null,
           statusCode: (int)HttpStatusCode.OK
       );
    }

    public async Task<Response<List<HospitalResponse>>> GetAllHospitalByFnAsync()
    {
        List<HospitalResponse> hospitals = await doctorProfileRepository.GetAllHospitalByFnAsync();

        return ResponseHelper.Response(
          data: hospitals,
          succeeded: true,
          message: SuccessMessages.RETRIEVED,
          errors: null,
          statusCode: (int)HttpStatusCode.OK
      );
    }

    public async Task<Response<int>> AddHospitalBySp(HospitalCreateRequest hospitalCreateRequest)
    {
        int hospital = await doctorProfileRepository.CreateHospitalAsync(hospitalCreateRequest.Name);

        return ResponseHelper.Response(
                 data: hospital,
                 succeeded: true,
                 message: SuccessMessages.RETRIEVED,
                 errors: null,
                 statusCode: (int)HttpStatusCode.OK
             );
    }

     public async Task<Response<DoctorProfileResponse>> AddDoctorProfileBySp(DoctorProfileRequest doctorProfileRequest, int userId)
    {
        DoctorProfileResponse doctorProfileResponse = await doctorProfileRepository.CreateDoctorProfileAsync(doctorProfileRequest,userId);

        return ResponseHelper.Response(
                 data: doctorProfileResponse,
                 succeeded: true,
                 message: SuccessMessages.RETRIEVED,
                 errors: null,
                 statusCode: (int)HttpStatusCode.OK
             );
    }
}
