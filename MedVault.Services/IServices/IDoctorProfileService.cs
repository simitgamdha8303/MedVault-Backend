using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;

namespace MedVault.Services.IServices;

public interface IDoctorProfileService
{
    Task<Response<string>> CreateAsync(DoctorProfileRequest request, int userId);
    Task<Response<DoctorProfileResponse>> GetByIdAsync(int id);
    Task<Response<string>> UpdateAsync(int id, DoctorProfileRequest request);
    Task<Response<string>> DeleteAsync(int id);
}
