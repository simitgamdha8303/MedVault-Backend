using System.Linq.Expressions;
using System.Net;
using AutoMapper;
using Humanizer;
using MedVault.Common.Helper;
using MedVault.Common.Messages;
using MedVault.Common.Response;
using MedVault.Data.IRepositories;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;
using MedVault.Models.Enums;
using MedVault.Services.IServices;
using MedVault.Utilities;

namespace MedVault.Services.Services;

public class MedicalTimelineService(
    IMedicalTimelineRepository medicalTimelineRepository,
    IPatientProfileRepository patientProfileRepository,
    IDoctorProfileRepository doctorProfileRepository,
    IDocumentRepository documentRepository,
    IMapper mapper
) : IMedicalTimelineService
{
    public async Task<Response<int>> CreateAsync(MedicalTimelineRequest medicalTimelineRequest, int userId)
    {

        PatientProfile? patientProfile = await patientProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId);
        if (patientProfile == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));
        }

        if (medicalTimelineRequest.DoctorProfileId.HasValue && !string.IsNullOrWhiteSpace(medicalTimelineRequest.DoctorName))
        {
            medicalTimelineRequest.DoctorName = null;
        }

        if (!medicalTimelineRequest.DoctorProfileId.HasValue && string.IsNullOrWhiteSpace(medicalTimelineRequest.DoctorName))
        {
            throw new ArgumentException(ErrorMessages.DOCTOR_PROFILE_NOT_SELECTED);
        }

        if (medicalTimelineRequest.DoctorProfileId.HasValue)
        {
            bool doctorExists = await doctorProfileRepository.AnyAsync(d => d.Id == medicalTimelineRequest.DoctorProfileId);
            if (!doctorExists)
            {
                throw new ArgumentException(ErrorMessages.NotFound("Doctor profile"));
            }
        }

        MedicalTimeline timeline = mapper.Map<MedicalTimeline>(medicalTimelineRequest);
        timeline.PatientId = patientProfile.Id;
        timeline.CreatedAt = DateTime.UtcNow;

        await medicalTimelineRepository.AddAsync(timeline);

        return ResponseHelper.Response<int>(
            data: timeline.Id,
            succeeded: true,
            message: SuccessMessages.Created("Medical timeline"),
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<MedicalTimelineResponse>> GetByIdAsync(int id)
    {
        MedicalTimelineResponse? timelineResponse = await medicalTimelineRepository.FirstOrDefaultAsync(
    x => x.Id == id,
    X => new MedicalTimelineResponse
    {
        Id = X.Id,
        PatientId = X.PatientId,
        DoctorProfileId = X.DoctorProfileId,
        CheckupTypeId = X.CheckupType,
        DoctorName = X.DoctorName,
        CheckupType = X.CheckupType.ToString(),
        EventDate = X.EventDate,
        Notes = X.Notes,
        DocumentResponses = X.Documents.Select(
            d => new DocumentResponse
            {
                FileName = d.FileName,
                FileUrl = d.FileUrl
            }
        ).ToList()
    });

        if (timelineResponse == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Medical timeline"));
        }

        return ResponseHelper.Response(
            data: timelineResponse,
            succeeded: true,
            message: SuccessMessages.RETRIEVED,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<int>> UpdateAsync(int id, MedicalTimelineRequest medicalTimelineRequest)
    {
        MedicalTimeline? timeline = await medicalTimelineRepository.GetByIdAsync(id);

        if (timeline == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Medical timeline"));
        }

        timeline.CheckupType = medicalTimelineRequest.CheckupType;
        timeline.EventDate = medicalTimelineRequest.EventDate;
        timeline.Notes = medicalTimelineRequest.Notes;
        timeline.DoctorProfileId = medicalTimelineRequest.DoctorProfileId;
        timeline.DoctorName = medicalTimelineRequest.DoctorName;
        timeline.UpdatedAt = DateTime.UtcNow;

        medicalTimelineRepository.Update(timeline);
        await medicalTimelineRepository.SaveChangesAsync();

        return ResponseHelper.Response<int>(
            data: timeline.Id,
            succeeded: true,
            message: SuccessMessages.Updated("Medical timeline"),
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string>> DeleteAsync(int id)
    {
        MedicalTimeline? timeline = await medicalTimelineRepository.GetByIdAsync(id);

        if (timeline == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Medical timeline"));
        }

        medicalTimelineRepository.Delete(timeline);
        await medicalTimelineRepository.SaveChangesAsync();

        return ResponseHelper.Response<string>(
            data: null,
            succeeded: true,
            message: SuccessMessages.Deleted("Medical timeline"),
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<List<MedicalTimelineResponse>>> GetFilteredAsync(int userId, TimelineSearchFilterRequest searchRequest)
    {
        PatientProfile? patientProfile = await patientProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId);
        if (patientProfile == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));
        }

        string doctor = searchRequest.Doctor?.Trim().ToLower() ?? "";

        Expression<Func<MedicalTimeline, bool>> predicate =
    x => x.PatientId == patientProfile.Id

        && (!searchRequest.CheckupType.HasValue
            || x.CheckupType == searchRequest.CheckupType.Value)

        && (
            string.IsNullOrWhiteSpace(doctor)
            || (
                (x.DoctorProfile != null &&
                 (
                     x.DoctorProfile.User.FirstName.ToLower().Contains(doctor) ||
                     x.DoctorProfile.User.LastName.ToLower().Contains(doctor)
                 ))
                || (x.DoctorName != null &&
                    x.DoctorName.ToLower().Contains(doctor))
            )
        )

        && (!searchRequest.FromDate.HasValue
            || x.EventDate >= searchRequest.FromDate.Value)

        && (!searchRequest.ToDate.HasValue
            || x.EventDate <= searchRequest.ToDate.Value);

        List<MedicalTimelineResponse> timelines = await medicalTimelineRepository.GetListAsync(
            predicate,
            X => new MedicalTimelineResponse
            {
                Id = X.Id,
                PatientId = X.PatientId,
                DoctorProfileName = X.DoctorProfile != null ? X.DoctorProfile.User.FirstName + " " + X.DoctorProfile.User.LastName : null,
                DoctorName = X.DoctorName,
                CheckupType = X.CheckupType.ToString(),
                EventDate = X.EventDate,
                Notes = X.Notes,
            }
        );

        return ResponseHelper.Response(
            data: timelines,
            succeeded: true,
            message: "Medical timeline retrieved",
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<int>> AddDocumentAsync(DocumentRequest documentRequest, int userId)
    {
        PatientProfile? patient = await patientProfileRepository
            .FirstOrDefaultAsync(p => p.UserId == userId);

        if (patient == null)
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));

        bool timelineExists = await medicalTimelineRepository
            .AnyAsync(t => t.Id == documentRequest.MedicalTimelineId && t.PatientId == patient.Id);

        if (!timelineExists)
            throw new ArgumentException(ErrorMessages.NotFound("Medical Timeline"));

        DocumentType documentType = DocumentTypeHelper.Detect(documentRequest.FileName);

        Document document = new()
        {
            PatientId = patient.Id,
            MedicalTimelineId = documentRequest.MedicalTimelineId,
            FileName = documentRequest.FileName,
            FileUrl = documentRequest.FileUrl,
            DocumentType = documentType,
            DocumentDate = documentRequest.DocumentDate,
            UploadedAt = DateTime.UtcNow
        };

        await documentRepository.AddAsync(document);

        return ResponseHelper.Response(
            data: document.Id,
            succeeded: true,
            message: SuccessMessages.Upload("Document"),
            errors: null,
            statusCode: 200
        );
    }
}

