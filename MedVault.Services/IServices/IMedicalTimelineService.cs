using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;

namespace MedVault.Services.IServices;

public interface IMedicalTimelineService
{
    Task<Response<string>> CreateAsync(MedicalTimelineRequest medicalTimelineRequest, int userId);
    Task<Response<MedicalTimelineResponse>> GetByIdAsync(int id);
    Task<Response<string>> UpdateAsync(int id, MedicalTimelineRequest medicalTimelineRequest);
    Task<Response<string>> DeleteAsync(int id);
    Task<Response<List<MedicalTimelineResponse>>> GetByPatientIdAsync(int patientId);
}
