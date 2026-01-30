namespace MedVault.Services.IServices;

using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;

public interface IAppointmentService
{
    Task<Response<string>> BookAsync(int userId, BookAppointmentRequest bookAppointmentRequest);

    Task<Response<List<AppointmentResponse>>> GetByPatientAsync(int userId);

    Task<Response<List<AppointmentResponse>>> GetByDoctorAsync(int userId);

    Task<Response<string>> ApproveAsync(int appointmentId, int doctorUserId);

    Task<Response<string>> RejectAsync(int appointmentId, int doctorUserId);

    Task<Response<string>> DeleteAsync(int appointmentId, int userId);

    Task<Response<string>> UpdateAsync(int appointmentId, int userId, BookAppointmentRequest bookAppointmentRequest);
    Task<Response<string>> CompleteAsync(int id, int doctorUserId);
    Task<Response<string>> CancelByDoctorAsync(int id, int doctorUserId);
}
