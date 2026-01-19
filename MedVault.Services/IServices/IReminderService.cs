using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;

namespace MedVault.Services.IServices;

public interface IReminderService
{
    Task<Response<int>> CreateAsync(CreateReminderRequest createReminderRequest, int userId);
    Task<Response<ReminderResponse>> GetByIdAsync(int id);
    Task<Response<List<ReminderResponse>>> GetByPatientAsync(int userId);
    Task<Response<int>> UpdateAsync(int id, UpdateReminderRequest updateReminderRequest);
    Task<Response<string>> DeleteAsync(int id);
}
