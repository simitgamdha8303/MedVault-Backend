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

public class PatientProfileService(
    IPatientProfileRepository patientProfileRepository,
    IUserRepository userRepository,
    IMapper mapper
) : IPatientProfileService
{
    public async Task<Response<string>> CreateAsync(PatientProfileRequest patientProfileRequest)
    {
        // Validate User
        bool userExists = await userRepository.AnyAsync(u => u.Id == patientProfileRequest.UserId);
        if (!userExists)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        bool patientProfileExists = await patientProfileRepository
            .AnyAsync(p => p.UserId == patientProfileRequest.UserId);

        if (patientProfileExists)
        {
            throw new ArgumentException(ErrorMessages.AlreadyExists("Patient profile"));
        }

        PatientProfile patientProfile = mapper.Map<PatientProfile>(patientProfileRequest);
        patientProfile.CreatedAt = DateTime.UtcNow;

        await patientProfileRepository.AddAsync(patientProfile);

        return ResponseHelper.Response<string>(
            data: null,
            succeeded: true,
            message: SuccessMessages.ProfileCreated("Patient"),
            errors: null,
            statusCode: (int)HttpStatusCode.Created
        );
    }

    public async Task<Response<PatientProfileResponse>> GetByIdAsync(int id)
    {
        PatientProfile? patientProfile = await patientProfileRepository.GetByIdAsync(id);

        if (patientProfile == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient profile"));
        }

        PatientProfileResponse patientProfileResponse =
            mapper.Map<PatientProfileResponse>(patientProfile);

        return ResponseHelper.Response<PatientProfileResponse>(
            data: patientProfileResponse,
            succeeded: true,
            message: SuccessMessages.ProfileRetrieved("Patient"),
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string>> UpdateAsync(int id, PatientProfileRequest request)
    {
        PatientProfile? patientProfile = await patientProfileRepository.GetByIdAsync(id);

        if (patientProfile == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient profile"));
        }

        mapper.Map(request, patientProfile);
        patientProfile.UpdatedAt = DateTime.UtcNow;

        patientProfileRepository.Update(patientProfile);
        await patientProfileRepository.SaveChangesAsync();

        return ResponseHelper.Response<string>(
            data: null,
            succeeded: true,
            message: SuccessMessages.Updated("Patient profile"),
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        PatientProfile? patientProfile = await patientProfileRepository.GetByIdAsync(id);

        if (patientProfile == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient profile"));
        }

        patientProfileRepository.Delete(patientProfile);
        await patientProfileRepository.SaveChangesAsync();

        return ResponseHelper.Response<string>(
            data: null,
            succeeded: true,
            message: SuccessMessages.Deleted("Patient profile"),
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }
}
