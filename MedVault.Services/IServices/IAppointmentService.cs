namespace MedVault.Services.IServices;

using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;

public interface IAppointmentService
{
    Task<Response<string>> BookAsync(int userId, BookAppointmentRequest request);

    Task<Response<List<AppointmentResponse>>> GetByPatientAsync(int userId);

    Task<Response<List<AppointmentResponse>>> GetByDoctorAsync(int userId);

    Task<Response<string>> ApproveAsync(int appointmentId, int doctorUserId);

    Task<Response<string>> RejectAsync(int appointmentId, int doctorUserId);
}
