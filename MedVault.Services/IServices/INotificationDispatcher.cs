using MedVault.Models.Entities;

public interface INotificationDispatcher
{
    Task SendReminderAsync(Reminder reminder);
    Task SendAppointmentUpdatedAsync(
        int appointmentId,
        int patientUserId,
        int doctorUserId
    );

    Task SendQrShareUpdatedAsync(
       Guid qrShareId,
       int patientUserId,
       int doctorUserId
   );
}
