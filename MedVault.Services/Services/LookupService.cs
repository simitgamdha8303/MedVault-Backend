using System.Net;
using AutoMapper;
using MedVault.Common.Helper;
using MedVault.Common.Messages;
using MedVault.Common.Response;
using MedVault.Data.IRepositories;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;
using MedVault.Models.Enums;
using MedVault.Services.IServices;

namespace MedVault.Services.Services;

public class LookupService(IDoctorProfileRepository doctorProfileRepository, IMapper mapper, IHospitalRepository hospitalRepository) : ILookupService
{
    public async Task<Response<List<EnumLookupResponse>>> GetAllDoctorAsync()
    {

        List<EnumLookupResponse>? doctorProfiles = await doctorProfileRepository.GetListAsync(
            x => 1 == 1,
            x => new EnumLookupResponse
            {
                Id = x.Id,
                Name = x.User.FirstName
            }
        );

        if (doctorProfiles == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Doctor"));
        }



        return ResponseHelper.Response(
            data: doctorProfiles,
            succeeded: true,
            message: SuccessMessages.RETRIEVED,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public Response<List<EnumLookupResponse>> GetCheckupTypes()
    {
        List<EnumLookupResponse>? checkupTypes = Enum.GetValues<CheckupType>()
            .Select(g => new EnumLookupResponse
            {
                Id = (int)g,
                Name = g.ToString()
            })
            .ToList();

        return ResponseHelper.Response(
            data: checkupTypes,
            succeeded: true,
            message: SuccessMessages.RETRIEVED,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public Response<List<EnumLookupResponse>> GetGenders()
    {
        List<EnumLookupResponse>? genders = Enum.GetValues<Gender>()
            .Select(g => new EnumLookupResponse
            {
                Id = (int)g,
                Name = g.ToString()
            })
            .ToList();

        return ResponseHelper.Response(
            data: genders,
            succeeded: true,
            message: SuccessMessages.RETRIEVED,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public Response<List<EnumLookupResponse>> GetBloodGroups()
    {
        List<EnumLookupResponse>? bloodGroups = Enum.GetValues<BloodGroup>()
            .Select(b => new EnumLookupResponse
            {
                Id = (int)b,
                Name = b.ToString().Replace("_", " ")
            })
            .ToList();

        return ResponseHelper.Response(
            data: bloodGroups,
            succeeded: true,
            message: SuccessMessages.RETRIEVED,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<List<EnumLookupResponse>>> GetAllHospitalAsync()
    {

        List<EnumLookupResponse>? hospitals = await hospitalRepository.GetListAsync(
            x => 1 == 1,
            x => new EnumLookupResponse
            {
                Id = x.Id,
                Name = x.Name
            }
        );

        if (hospitals == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Hospital"));
        }

        return ResponseHelper.Response(
            data: hospitals,
            succeeded: true,
            message: SuccessMessages.RETRIEVED,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }
}