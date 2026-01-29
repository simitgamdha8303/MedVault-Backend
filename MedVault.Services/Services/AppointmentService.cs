using System.Net;
using MedVault.Common.Helper;
using MedVault.Common.Messages;
using MedVault.Common.Response;
using MedVault.Data.IRepositories;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;
using MedVault.Models.Enums;
using MedVault.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace MedVault.Services.Services;

public class AppointmentService(
    IGenericRepository<Appointment> appointmentRepository,
    IPatientProfileRepository patientProfileRepository,
    IDoctorProfileRepository doctorProfileRepository
) : IAppointmentService
{
    public async Task<Response<string>> BookAsync(int userId, BookAppointmentRequest bookAppointmentRequest)
    {
        PatientProfile? patient = await patientProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId);

        if (patient == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));
        }

        bool doctorExists = await doctorProfileRepository.AnyAsync(d => d.Id == bookAppointmentRequest.DoctorId);

        if (!doctorExists)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Doctor"));
        }

        Appointment appointment = new()
        {
            PatientId = patient.Id,
            DoctorId = bookAppointmentRequest.DoctorId,
            AppointmentDate = DateTime.SpecifyKind(
        bookAppointmentRequest.AppointmentDate.Date,
        DateTimeKind.Utc
    ),
            AppointmentTime = bookAppointmentRequest.AppointmentTime,
            CheckupType = bookAppointmentRequest.CheckupType,
            Status = AppointmentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        await appointmentRepository.AddAsync(appointment);

        return ResponseHelper.Response(
            appointment.Id.ToString(),
            true,
            SuccessMessages.Created("Appointment"),
            null,
            (int)HttpStatusCode.Created
        );
    }

    public async Task<Response<List<AppointmentResponse>>> GetByPatientAsync(int userId)
    {
        PatientProfile? patient = await patientProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId);

        if (patient == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));
        }

        List<AppointmentResponse> appointments =
            await appointmentRepository.GetListAsync(
                a => a.PatientId == patient.Id,
                a => new AppointmentResponse
                {
                    Id = a.Id,
                    DoctorName = a.DoctorProfile.User.FirstName + " " +
                                 a.DoctorProfile.User.LastName,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    CheckupType = a.CheckupType,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt
                }
            );

        return ResponseHelper.Response(
            appointments.OrderByDescending(x => x.CreatedAt).ToList(),
            true,
            SuccessMessages.RETRIEVED,
            null,
            (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<List<AppointmentResponse>>> GetByDoctorAsync(int userId)
    {
        DoctorProfile? doctor =
            await doctorProfileRepository.FirstOrDefaultAsync(d => d.UserId == userId);

        if (doctor == null)
            throw new ArgumentException(ErrorMessages.NotFound("Doctor"));

        List<AppointmentResponse> appointments =
            await appointmentRepository.GetListAsync(
                a => a.DoctorId == doctor.Id,
                a => new AppointmentResponse
                {
                    Id = a.Id,
                    PatientName = a.PatientProfile.User.FirstName + " " +
                                  a.PatientProfile.User.LastName,
                    AppointmentDate = a.AppointmentDate,
                    AppointmentTime = a.AppointmentTime,
                    CheckupType = a.CheckupType,
                    Status = a.Status,
                    CreatedAt = a.CreatedAt
                }
            );

        return ResponseHelper.Response(
            appointments.OrderBy(x => x.AppointmentDate).ToList(),
            true,
            SuccessMessages.RETRIEVED,
            null,
            (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string>> ApproveAsync(int appointmentId, int doctorUserId)
    {
        DoctorProfile? doctor = await doctorProfileRepository.FirstOrDefaultAsync(d => d.UserId == doctorUserId);

        if (doctor == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Doctor"));
        }

        Appointment? appointment = await appointmentRepository.Query().FirstOrDefaultAsync(a => a.Id == appointmentId && a.DoctorId == doctor.Id);

        if (appointment == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Appointment"));
        }

        appointment.Status = AppointmentStatus.Confirmed;
        appointment.UpdatedAt = DateTime.UtcNow;

        appointmentRepository.Update(appointment);
        await appointmentRepository.SaveChangesAsync();

        return ResponseHelper.Response<string>(
            null,
            true,
            "Appointment approved successfully",
            null,
            (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string>> RejectAsync(int appointmentId, int doctorUserId)
    {
        DoctorProfile? doctor = await doctorProfileRepository.FirstOrDefaultAsync(d => d.UserId == doctorUserId);

        if (doctor == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Doctor"));
        }

        Appointment? appointment = await appointmentRepository.Query().FirstOrDefaultAsync(a => a.Id == appointmentId && a.DoctorId == doctor.Id);

        if (appointment == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Appointment"));
        }

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.UpdatedAt = DateTime.UtcNow;

        appointmentRepository.Update(appointment);
        await appointmentRepository.SaveChangesAsync();

        return ResponseHelper.Response<string>(
            null,
            true,
            "Appointment rejected",
            null,
            (int)HttpStatusCode.OK
        );
    }
}
