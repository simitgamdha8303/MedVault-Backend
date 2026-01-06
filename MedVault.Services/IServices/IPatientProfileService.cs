using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;

namespace MedVault.Services.IServices;

public interface IPatientProfileService
{
    Task<Response<string>> CreateAsync(PatientProfileRequest request);
    Task<Response<PatientProfileResponse>> GetByIdAsync(int id);
    Task<Response<string>> UpdateAsync(int id, PatientProfileRequest request);
    Task<Response<string>> DeleteAsync(int id);
}
