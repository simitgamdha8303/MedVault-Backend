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
    IDoctorProfileRepository doctorProfileRepository,
     INotificationDispatcher notificationDispatcher
) : IAppointmentService
{
    public async Task<Response<string>> BookAsync(int userId, BookAppointmentRequest bookAppointmentRequest)
    {
        PatientProfile? patient = await patientProfileRepository.Query()
        .Where(p => p.UserId == userId)
        .Select(p => new PatientProfile { Id = p.Id })
        .FirstAsync(); ;

        if (patient == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));
        }

        bool doctorExists = await doctorProfileRepository.AnyAsync(d => d.Id == bookAppointmentRequest.DoctorId);

        if (!doctorExists)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Doctor"));
        }

        int doctorUserId = await doctorProfileRepository
      .Query()
      .Where(d => d.Id == bookAppointmentRequest.DoctorId)
      .Select(d => d.UserId)
      .FirstAsync();

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

        await notificationDispatcher.SendAppointmentUpdatedAsync(
       appointment.Id,
       userId,
       doctorUserId
   );

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
                    DoctorId = a.DoctorId,
                    DoctorName = a.DoctorProfile.User.FirstName + " " +
                                 a.DoctorProfile.User.LastName,
                    AppointmentDate = DateTime.SpecifyKind(
    a.AppointmentDate,
    DateTimeKind.Utc
).ToLocalTime(),

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

        await notificationDispatcher.SendAppointmentUpdatedAsync(
     appointment.Id,
     await patientProfileRepository
         .Query()
         .Where(p => p.Id == appointment.PatientId)
         .Select(p => p.UserId)
         .FirstAsync(),
     doctorUserId
 );


        return ResponseHelper.Response<string>(
            null,
            true,
            SuccessMessages.APPOINTMENT_APPROVED,
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

        await notificationDispatcher.SendAppointmentUpdatedAsync(
     appointment.Id,
     await patientProfileRepository
         .Query()
         .Where(p => p.Id == appointment.PatientId)
         .Select(p => p.UserId)
         .FirstAsync(),
     doctorUserId
 );


        return ResponseHelper.Response<string>(
            null,
            true,
            SuccessMessages.APPOINTMENT_REJECTED,
            null,
            (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string>> UpdateAsync(int appointmentId, int userId, BookAppointmentRequest bookAppointmentRequest)
    {
        PatientProfile? patient =
            await patientProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId);

        if (patient == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));
        }

        Appointment? appointment =
            await appointmentRepository.Query()
                .FirstOrDefaultAsync(a => a.Id == appointmentId && a.PatientId == patient.Id);

        if (appointment == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Appointment"));
        }

        if (appointment.Status != AppointmentStatus.Pending)
        {
            return ResponseHelper.Response<string>(
                null,
                false,
                SuccessMessages.APPOINTMENT_CANNOT_BE_UPDATED,
                null,
                (int)HttpStatusCode.BadRequest
            );
        }

        appointment.AppointmentDate = DateTime.SpecifyKind(
            bookAppointmentRequest.AppointmentDate.Date,
            DateTimeKind.Utc
        );
        appointment.AppointmentTime = bookAppointmentRequest.AppointmentTime;
        appointment.CheckupType = bookAppointmentRequest.CheckupType;
        appointment.UpdatedAt = DateTime.UtcNow;

        appointmentRepository.Update(appointment);
        await appointmentRepository.SaveChangesAsync();

        await notificationDispatcher.SendAppointmentUpdatedAsync(
    appointment.Id,
    userId,
    await doctorProfileRepository
        .Query()
        .Where(d => d.Id == appointment.DoctorId)
        .Select(d => d.UserId)
        .FirstAsync()
);


        return ResponseHelper.Response<string>(
            null,
            true,
           SuccessMessages.Updated("Appointment"),
            null,
            (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string>> DeleteAsync(int appointmentId, int userId)
    {
        PatientProfile? patient =
            await patientProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId);

        if (patient == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));
        }

        Appointment? appointment =
            await appointmentRepository.Query()
                .FirstOrDefaultAsync(a => a.Id == appointmentId && a.PatientId == patient.Id);

        if (appointment == null)
            throw new ArgumentException(ErrorMessages.NotFound("Appointment"));

        if (appointment.Status == AppointmentStatus.Confirmed)
        {
            return ResponseHelper.Response<string>(
                null,
                false,
                SuccessMessages.APPOINTMENT_CANNOT_BE_DELETED,
                null,
                (int)HttpStatusCode.BadRequest
            );
        }

        int doctorUserId = await doctorProfileRepository
     .Query()
     .Where(d => d.Id == appointment.DoctorId)
     .Select(d => d.UserId)
     .FirstAsync();

        appointmentRepository.Delete(appointment);
        await appointmentRepository.SaveChangesAsync();

        await notificationDispatcher.SendAppointmentUpdatedAsync(
     appointment.Id,
     userId,
     doctorUserId
 );

        return ResponseHelper.Response<string>(
            null,
            true,
            SuccessMessages.Deleted("Appointment"),
            null,
            (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string>> CompleteAsync(int id, int doctorUserId)
    {
        DoctorProfile? doctor = await doctorProfileRepository.FirstOrDefaultAsync(d => d.UserId == doctorUserId);
        Appointment? appointment = await appointmentRepository
           .Query()
           .FirstOrDefaultAsync(a => a.Id == id && a.DoctorId == doctor.Id);

        appointment.Status = AppointmentStatus.Completed;
        appointment.UpdatedAt = DateTime.UtcNow;

        appointmentRepository.Update(appointment);
        await appointmentRepository.SaveChangesAsync();

        int patientUserId = await patientProfileRepository
     .Query()
     .Where(p => p.Id == appointment.PatientId)
     .Select(p => p.UserId)
     .FirstAsync();

        await notificationDispatcher.SendAppointmentUpdatedAsync(
            appointment.Id,
            patientUserId,
            doctorUserId
        );


        return ResponseHelper.Response<string>(null, true, "Appointment completed", null, 200);
    }

    public async Task<Response<string>> CancelByDoctorAsync(int id, int doctorUserId)
    {
        DoctorProfile? doctor = await doctorProfileRepository.FirstOrDefaultAsync(d => d.UserId == doctorUserId);
        Appointment? appointment = await appointmentRepository
           .Query()
           .FirstOrDefaultAsync(a => a.Id == id && a.DoctorId == doctor.Id);

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.UpdatedAt = DateTime.UtcNow;

        appointmentRepository.Update(appointment);
        await appointmentRepository.SaveChangesAsync();

        int patientUserId = await patientProfileRepository
    .Query()
    .Where(p => p.Id == appointment.PatientId)
    .Select(p => p.UserId)
    .FirstAsync();

        await notificationDispatcher.SendAppointmentUpdatedAsync(
            appointment.Id,
            patientUserId,
            doctorUserId
        );


        return ResponseHelper.Response<string>(null, true, "Appointment cancelled", null, 200);
    }

}
