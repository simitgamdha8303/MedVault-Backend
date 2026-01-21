using System.Globalization;
using System.Net;
using MedVault.Common.Helper;
using MedVault.Common.Messages;
using MedVault.Common.Response;
using MedVault.Data.IRepositories;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;
using MedVault.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace MedVault.Services.Services;

public class DashboardService(IPatientProfileRepository patientProfileRepository, IMedicalTimelineRepository medicalTimelineRepository, IReminderRepository reminderRepository) : IDashboardService
{
    public async Task<Response<PatientLastVisitResponse>> GetLastVisit(int? userId)
    {
        if (!userId.HasValue)
        {
            throw new ArgumentException(ErrorMessages.NotFound("UserId"));
        }

        PatientProfile? patientProfile = await patientProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId);
        if (patientProfile == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));
        }



        PatientLastVisitResponse? lastVisit =
     await medicalTimelineRepository
         .Query()
         .Where(m => m.PatientId == patientProfile.Id)
         .OrderByDescending(m => m.EventDate)
         .Select(m => new PatientLastVisitResponse
         {
             VisitDate = m.EventDate.ToString("dd MMM yyyy"),

             DocotrName =
                 !string.IsNullOrEmpty(m.DoctorName)
                     ? m.DoctorName
                     : (
                         m.DoctorProfile != null &&
                         m.DoctorProfile.User != null
                             ? m.DoctorProfile.User.FirstName + " " +
                               m.DoctorProfile.User.LastName
                             : "Unknown Doctor"
                       )
         })
         .FirstOrDefaultAsync();

        if (lastVisit == null)
        {
            return ResponseHelper.Response(
                data: lastVisit,
                succeeded: true,
                message: "No visits found",
                errors: null,
                statusCode: (int)HttpStatusCode.OK
            );
        }

        return ResponseHelper.Response(
            data: lastVisit,
            succeeded: true,
            message: SuccessMessages.RETRIEVED,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<int>> GetMedicalTimelineCount(int? userId)
    {
        if (!userId.HasValue)
        {
            throw new ArgumentException(ErrorMessages.NotFound("UserId"));
        }

        PatientProfile? patientProfile = await patientProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId);
        if (patientProfile == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));
        }

        int count = await medicalTimelineRepository
            .Query()
            .CountAsync(m => m.PatientId == patientProfile.Id);

        return ResponseHelper.Response(
            data: count,
            succeeded: true,
            message: SuccessMessages.RETRIEVED,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );

    }

    public async Task<Response<string>> GetUpcomingAppointment(int? userId)
    {
        if (!userId.HasValue)
            throw new ArgumentException(ErrorMessages.NotFound("UserId"));

        PatientProfile? patientProfile =
            await patientProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId.Value);

        if (patientProfile == null)
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));

        DateTime now = DateTime.UtcNow;

        DateTime? upcomingAppointment =
            await reminderRepository
                .Query()
                .Where(r =>
                    r.PatientId == patientProfile.Id &&
                    r.ReminderTime > now && r.ReminderTypeId == 2
                )
                .OrderBy(r => r.ReminderTime)
                .Select(r => (DateTime?)r.ReminderTime)
                .FirstOrDefaultAsync();

        if (!upcomingAppointment.HasValue)
        {
            return ResponseHelper.Response(
                data: "No upcoming appointments",
                succeeded: true,
                message: SuccessMessages.RETRIEVED,
                errors: null,
                statusCode: (int)HttpStatusCode.OK
            );
        }

        return ResponseHelper.Response(
            data: upcomingAppointment.Value.ToString("dd MMM yyyy"),
            succeeded: true,
            message: SuccessMessages.RETRIEVED,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<List<VisitChartPointResponse>>> GetVisitChart(
     int userId,
     string filter)
    {
        PatientProfile patient =
            await patientProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId)
            ?? throw new ArgumentException("Patient not found");

        var data = await medicalTimelineRepository
            .Query()
            .Where(m => m.PatientId == patient.Id)
            .Select(m => new
            {
                m.EventDate,
                Doctor = !string.IsNullOrWhiteSpace(m.DoctorName)
                ? m.DoctorName
                : m.DoctorProfile != null &&
                  m.DoctorProfile.User != null &&
                  !string.IsNullOrWhiteSpace(m.DoctorProfile.User.FirstName)
                    ? m.DoctorProfile.User.FirstName
                    : "Unknown Doctor"
            })
            .ToListAsync();

        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
        List<VisitChartPointResponse> result = new();

        // CURRENT MONTH
        if (filter == "current-month")
        {
            var start = new DateOnly(today.Year, today.Month, 1);
            var end = start.AddMonths(1).AddDays(-1);

            result = GetDateRange(start, end)
                        .Select(d =>
                        {
                            var items = data.Where(x => x.EventDate == d).ToList();
                            return new VisitChartPointResponse
                            {
                                Label = d.Day.ToString(),
                                Count = items.Count,
                                Doctors = items.Select(x => x.Doctor).Distinct().ToList()
                            };
                        })
                        .ToList();
        }

        // LAST 3 MONTHS
        else if (filter == "last-3-months")
        {
            for (int i = 2; i >= 0; i--)
            {
                var month = today.AddMonths(-i);
                var items = data.Where(x =>
                    x.EventDate.Year == month.Year &&
                    x.EventDate.Month == month.Month
                );

                result.Add(new VisitChartPointResponse
                {
                    Label = month.ToString("MMM yyyy"),
                    Count = items.Count(),
                    Doctors = items.Select(x => x.Doctor).Distinct().ToList()
                });
            }
        }

        // CURRENT YEAR
        else if (filter == "current-year")
        {
            for (int m = 1; m <= 12; m++)
            {
                var items = data.Where(x =>
                    x.EventDate.Year == today.Year &&
                    x.EventDate.Month == m
                );

                result.Add(new VisitChartPointResponse
                {
                    Label = CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(m),
                    Count = items.Count(),
                    Doctors = items.Select(x => x.Doctor).Distinct().ToList()
                });
            }
        }

        // LAST YEAR
        else if (filter == "last-year")
        {
            int year = today.Year - 1;

            for (int m = 1; m <= 12; m++)
            {
                var items = data.Where(x =>
                    x.EventDate.Year == year &&
                    x.EventDate.Month == m
                );

                result.Add(new VisitChartPointResponse
                {
                    Label = $"{CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(m)} {year}",
                    Count = items.Count(),
                    Doctors = items.Select(x => x.Doctor).Distinct().ToList()
                });
            }
        }

        return ResponseHelper.Response(result, true, "Retrieved", null, 200);
    }

    private static List<DateOnly> GetDateRange(DateOnly start, DateOnly end)
    {
        var list = new List<DateOnly>();
        for (var date = start; date <= end; date = date.AddDays(1))
            list.Add(date);
        return list;
    }


}