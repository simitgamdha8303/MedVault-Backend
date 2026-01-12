using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;

namespace MedVault.Services.IServices;

public interface IMedicalTimelineService
{
    Task<Response<int>> CreateAsync(MedicalTimelineRequest medicalTimelineRequest, int userId);
    Task<Response<MedicalTimelineResponse>> GetByIdAsync(int id);
    Task<Response<int>> UpdateAsync(int id, MedicalTimelineRequest medicalTimelineRequest);
    Task<Response<string>> DeleteAsync(int id);
    Task<Response<List<MedicalTimelineResponse>>> GetFilteredAsync(int userId, TimelineSearchFilterRequest searchRequest);
    Task<Response<int>> AddDocumentAsync(DocumentRequest request, int userId);
}
