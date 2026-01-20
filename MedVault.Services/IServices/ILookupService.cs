using MedVault.Common.Response;
using MedVault.Models.Dtos.ResponseDtos;

namespace MedVault.Services.IServices;

public interface ILookupService
{
    Task<Response<List<EnumLookupResponse>>> GetAllDoctorAsync();
    Response<List<EnumLookupResponse>> GetCheckupTypes();
    Response<List<EnumLookupResponse>> GetGenders();
    Response<List<EnumLookupResponse>> GetBloodGroups();
    Task<Response<List<EnumLookupResponse>>> GetAllHospitalAsync();
    Task<Response<List<EnumLookupResponse>>> GetAllReminderTypeAsync();

    Response<List<EnumLookupResponse>> GetRecurrenceType();

}