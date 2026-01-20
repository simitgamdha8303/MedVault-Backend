using System.Net;
using AutoMapper;
using Hangfire;
using MedVault.Common.Helper;
using MedVault.Common.Messages;
using MedVault.Common.Response;
using MedVault.Data.IRepositories;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;
using MedVault.Services.IServices;

namespace MedVault.Services.Services;

public class ReminderService(IReminderRepository reminderRepository, IUserRepository userRepository, IPatientProfileRepository patientProfileRepository, IMapper mapper,
   IBackgroundJobClient backgroundJobs) : IReminderService
{
    public async Task<Response<int>> CreateAsync(CreateReminderRequest createReminderRequest, int userId)
    {
        bool userExists = await userRepository.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        PatientProfile? patient = await patientProfileRepository
           .FirstOrDefaultAsync(p => p.UserId == userId);

        if (patient == null)
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));

        if (createReminderRequest.RecurrenceEndDate != null &&
createReminderRequest.RecurrenceEndDate <= createReminderRequest.ReminderTime)
        {
            throw new ArgumentException(
                "Recurrence end date must be after reminder time");
        }


        Reminder reminder = mapper.Map<Reminder>(createReminderRequest);
        reminder.PatientId = patient.Id;
        reminder.IsActive = true;
        reminder.CreatedAt = DateTime.UtcNow;

        await reminderRepository.AddAsync(reminder);

        string jobId = backgroundJobs.Schedule<ReminderJobService>(
        j => j.ExecuteAsync(reminder.Id),
        reminder.ReminderTime
        );

        reminder.HangfireJobId = jobId;
        await reminderRepository.SaveChangesAsync();

        return ResponseHelper.Response(
            data: reminder.Id,
            succeeded: true,
            message: SuccessMessages.Created("Reminder"),
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<ReminderResponse>> GetByIdAsync(int id)
    {
        ReminderResponse? reminder = await reminderRepository.FirstOrDefaultAsync(
            r => r.Id == id,
            r => new ReminderResponse
            {
                Id = r.Id,
                PatientId = r.PatientId,
                ReminderType = r.ReminderType.Name,
                Title = r.Title,
                Description = r.Description,
                ReminderTime = r.ReminderTime,
                RecurrenceType = r.RecurrenceType.ToString(),
                RecurrenceInterval = r.RecurrenceInterval,
                RecurrenceEndDate = r.RecurrenceEndDate,
                IsActive = r.IsActive
            }
        );

        if (reminder == null)
            throw new ArgumentException(ErrorMessages.NotFound("Reminder"));

        return ResponseHelper.Response(
            reminder,
            true,
            SuccessMessages.RETRIEVED,
            null,
            (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<List<ReminderResponse>>> GetByPatientAsync(int userId)
    {
        bool userExists = await userRepository.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        PatientProfile? patient = await patientProfileRepository
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (patient == null)
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));

        List<ReminderResponse> reminders =
            (await reminderRepository.GetListAsync(
                r => r.PatientId == patient.Id,
                r => new ReminderResponse
                {
                    Id = r.Id,
                    PatientId = r.PatientId,
                    ReminderType = r.ReminderType.Name,
                    Title = r.Title,
                    Description = r.Description,
                    ReminderTime = r.ReminderTime,
                    RecurrenceType = r.RecurrenceType.ToString(),
                    RecurrenceInterval = r.RecurrenceInterval,
                    RecurrenceEndDate = r.RecurrenceEndDate,
                    IsActive = r.IsActive
                }
            ))
            .OrderBy(x => x.ReminderTime)
            .ToList();

        return ResponseHelper.Response(
            reminders,
            true,
            "Reminders retrieved",
            null,
            (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<int>> UpdateAsync(int id, UpdateReminderRequest request)
    {
        Reminder? reminder = await reminderRepository.GetByIdAsync(id);

        if (reminder == null)
            throw new ArgumentException(ErrorMessages.NotFound("Reminder"));

        // DELETE OLD JOB
        if (!string.IsNullOrEmpty(reminder.HangfireJobId))
        {
            BackgroundJob.Delete(reminder.HangfireJobId);
        }

        mapper.Map(request, reminder);

        reminder.UpdatedAt = DateTime.UtcNow;
        reminder.IsActive = true;

        // SCHEDULE NEW JOB
        string newJobId = BackgroundJob.Schedule<ReminderJobService>(
            j => j.ExecuteAsync(reminder.Id),
            reminder.ReminderTime
        );

        reminder.HangfireJobId = newJobId;

        reminderRepository.Update(reminder);
        await reminderRepository.SaveChangesAsync();

        return ResponseHelper.Response(
            reminder.Id,
            true,
            SuccessMessages.Updated("Reminder"),
            null,
            (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        Reminder? reminder = await reminderRepository.GetByIdAsync(id);

        if (reminder == null)
            throw new ArgumentException(ErrorMessages.NotFound("Reminder"));

        if (!string.IsNullOrEmpty(reminder.HangfireJobId))
        {
            BackgroundJob.Delete(reminder.HangfireJobId);
        }

        reminderRepository.Delete(reminder);
        await reminderRepository.SaveChangesAsync();

        return ResponseHelper.Response<string>(
            null,
            true,
            SuccessMessages.Deleted("Reminder"),
            null,
            (int)HttpStatusCode.OK
        );
    }
}
