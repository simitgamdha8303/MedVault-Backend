using MedVault.Common.Response;
using MedVault.Models.Dtos.ResponseDtos;

namespace MedVault.Services.IServices;

public interface IDashboardService
{
    Task<Response<int>> GetMedicalTimelineCount(int? userId);

    Task<Response<PatientLastVisitResponse>> GetLastVisit(int? userId);

    Task<Response<string>> GetUpcomingAppointment(int? userId);

    Task<Response<List<VisitChartPointResponse>>> GetVisitChart(int userId,string filter);

}