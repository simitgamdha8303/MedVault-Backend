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

public class DashboardService(IPatientProfileRepository patientProfileRepository, IDoctorProfileRepository doctorProfileRepository,
 IMedicalTimelineRepository medicalTimelineRepository, IReminderRepository reminderRepository) : IDashboardService
{
    public async Task<Response<PatientLastVisitResponse>> GetLastVisit(int userId)
    {

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
             VisitDate = m.EventDate.ToLocalTime().ToString("dd MMM yyyy"),

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
                message: ErrorMessages.NotFound("Visit"),
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

    public async Task<Response<int>> GetMedicalTimelineCount(int userId)
    {
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

    public async Task<Response<string>> GetUpcomingAppointment(int userId)
    {

        PatientProfile? patientProfile =
            await patientProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId);

        if (patientProfile == null)
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));

        DateTime now = DateTime.UtcNow;

        DateTime? upcomingAppointment =
            await reminderRepository
                .Query()
                .Where(r =>
                    r.PatientId == patientProfile.Id &&
                    r.ReminderTime > now
                )
                .OrderBy(r => r.ReminderTime)
                .Select(r => (DateTime?)r.ReminderTime)
                .FirstOrDefaultAsync();

        if (!upcomingAppointment.HasValue)
        {
            return ResponseHelper.Response(
                data: SuccessMessages.NO_APPOINTMENTS,
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
        PatientProfile? patient = await patientProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId);

        if (patient == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));
        }

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
            DateOnly start = new DateOnly(today.Year, today.Month, 1);
            DateOnly end = start.AddMonths(1).AddDays(-1);

            var filteredData = data.Where(x =>
                DateOnly.FromDateTime(x.EventDate) >= start &&
                DateOnly.FromDateTime(x.EventDate) <= end
            ).ToList();

            result = GetDateRange(start, end)
                        .Select(d =>
                        {
                            var items = filteredData
                                .Where(x => DateOnly.FromDateTime(x.EventDate.ToLocalTime()) == d)
                                .ToList();

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

        return ResponseHelper.Response(result, true, SuccessMessages.RETRIEVED, null, (int)HttpStatusCode.OK);
    }

    private static List<DateOnly> GetDateRange(DateOnly start, DateOnly end)
    {
        var list = new List<DateOnly>();
        for (var date = start; date <= end; date = date.AddDays(1))
            list.Add(date);
        return list;
    }


    public async Task<Response<DoctorLastCheckupResponse>> GetLastPatientCheckup(int userId)
    {
        DoctorProfile? doctor = await doctorProfileRepository.FirstOrDefaultAsync(d => d.UserId == userId);
        if (doctor == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Doctor"));
        }

        DoctorLastCheckupResponse? lastVisit = await medicalTimelineRepository
            .Query()
            .Where(m => m.DoctorProfileId == doctor.Id)
            .OrderByDescending(m => m.EventDate)
            .Select(m => new DoctorLastCheckupResponse
            {
                PatientName = m.PatientProfile.User.FirstName + " " + m.PatientProfile.User.LastName,
                VisitDate = m.EventDate.ToLocalTime().ToString("dd MMM yyyy")
            })
            .FirstOrDefaultAsync();

        return ResponseHelper.Response(lastVisit, true, SuccessMessages.RETRIEVED, null, (int)HttpStatusCode.OK);
    }

    public async Task<Response<int>> GetTotalPatientCheckups(int userId)
    {
        DoctorProfile? doctor = await doctorProfileRepository.FirstOrDefaultAsync(d => d.UserId == userId);
        if (doctor == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Doctor"));
        }

        int count = await medicalTimelineRepository
            .Query()
            .CountAsync(m => m.DoctorProfileId == doctor.Id);

        return ResponseHelper.Response(count, true, SuccessMessages.RETRIEVED, null, (int)HttpStatusCode.OK);
    }

    public async Task<Response<List<TopPatientResponse>>> GetTopPatients(int userId)
    {
        DoctorProfile? doctor = await doctorProfileRepository.FirstOrDefaultAsync(d => d.UserId == userId);
        if (doctor == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Doctor"));
        }

        List<TopPatientResponse>? topPatients = await medicalTimelineRepository
            .Query()
            .Where(m => m.DoctorProfileId == doctor.Id)
            .GroupBy(m => new
            {
                m.PatientProfile.User.FirstName,
                m.PatientProfile.User.LastName
            })
            .Select(g => new TopPatientResponse
            {
                PatientName = g.Key.FirstName + " " + g.Key.LastName,
                VisitCount = g.Count()
            })
            .OrderByDescending(x => x.VisitCount)
            .Take(3)
            .ToListAsync();

        return ResponseHelper.Response(topPatients, true, SuccessMessages.RETRIEVED, null, (int)HttpStatusCode.OK);
    }

    public async Task<Response<List<DoctorVisitChartPointResponse>>> GetPatientVisitChart(
        int userId,
        string filter)
    {
        DoctorProfile? doctor = await doctorProfileRepository.FirstOrDefaultAsync(d => d.UserId == userId);
        if (doctor == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Doctor"));
        }

        var data = await medicalTimelineRepository
            .Query()
            .Where(m => m.DoctorProfileId == doctor.Id)
            .Select(m => new
            {
                m.EventDate,
                Patient =
                    m.PatientProfile != null &&
                    m.PatientProfile.User != null
                        ? m.PatientProfile.User.FirstName + " " +
                          m.PatientProfile.User.LastName
                        : "Unknown Patient"
            })
            .ToListAsync();

        DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
        List<DoctorVisitChartPointResponse> result = new();

        // CURRENT MONTH (day-wise)
        if (filter == "current-month")
        {
            DateOnly start = new(today.Year, today.Month, 1);
            DateOnly end = start.AddMonths(1).AddDays(-1);

            var filteredData = data.Where(x =>
                DateOnly.FromDateTime(x.EventDate) >= start &&
                DateOnly.FromDateTime(x.EventDate) <= end);

            result = GetDateRange(start, end)
                .Select(d =>
                {
                    var items = filteredData
                        .Where(x => DateOnly.FromDateTime(x.EventDate.ToLocalTime()) == d)
                        .ToList();

                    return new DoctorVisitChartPointResponse
                    {
                        Label = d.Day.ToString(),
                        Count = items.Count,
                        Patients = items.Select(x => x.Patient).Distinct().ToList()
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
                    x.EventDate.Month == month.Month);

                result.Add(new DoctorVisitChartPointResponse
                {
                    Label = month.ToString("MMM yyyy"),
                    Count = items.Count(),
                    Patients = items.Select(x => x.Patient).Distinct().ToList()
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
                    x.EventDate.Month == m);

                result.Add(new DoctorVisitChartPointResponse
                {
                    Label = CultureInfo.CurrentCulture
                        .DateTimeFormat.GetAbbreviatedMonthName(m),
                    Count = items.Count(),
                    Patients = items.Select(x => x.Patient).Distinct().ToList()
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
                    x.EventDate.Month == m);

                result.Add(new DoctorVisitChartPointResponse
                {
                    Label = $"{CultureInfo.CurrentCulture
                        .DateTimeFormat.GetAbbreviatedMonthName(m)} {year}",
                    Count = items.Count(),
                    Patients = items.Select(x => x.Patient).Distinct().ToList()
                });
            }
        }

        return ResponseHelper.Response(result, true, SuccessMessages.RETRIEVED, null, 200);
    }




}