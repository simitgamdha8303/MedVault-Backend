using System.Net;
using System.Security.Cryptography;
using AutoMapper;
using MedVault.Common.Helper;
using MedVault.Common.Messages;
using MedVault.Common.Response;
using MedVault.Data.IRepositories;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;
using MedVault.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QRCoder;

namespace MedVault.Services.Services;

public class QrShareService(IQrShareRepository qrShareRepository,
 IConfiguration configuration,
 IPatientProfileRepository patientProfileRepository,
 IDoctorProfileRepository doctorProfileRepository,
 INotificationDispatcher notificationDispatcher,
 IUserRepository userRepository) : IQrShareService
{
    public async Task<Response<string>> GenerateAsync(
         int userId,
         GenerateQrRequest GenerateQrRequest)
    {

        PatientProfile? patientProfile = await patientProfileRepository.FirstOrDefaultAsync(x => x.UserId == userId);
        if (patientProfile == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient Profile"));
        }

        bool doctorExists = await doctorProfileRepository.AnyAsync(d => d.Id == GenerateQrRequest.DoctorId);
        if (!doctorExists)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Doctor"));
        }

        byte[]? tokenBytes = RandomNumberGenerator.GetBytes(64);
        string? accessToken = Convert.ToBase64String(tokenBytes);

        DateTime expiresAt = DateTime.UtcNow.AddMinutes(
            GenerateQrRequest.ExpiryMinutes > 0 ? GenerateQrRequest.ExpiryMinutes : 10);

        QrShare? qrShare = new QrShare
        {
            Id = Guid.NewGuid(),
            PatientId = patientProfile.Id,
            DoctorId = GenerateQrRequest.DoctorId,
            AccessToken = accessToken,
            ExpiresAt = expiresAt,
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };

        await qrShareRepository.AddAsync(qrShare);
        await qrShareRepository.SaveChangesAsync();

        int doctorUserId = await doctorProfileRepository
    .Query()
    .Where(d => d.Id == GenerateQrRequest.DoctorId)
    .Select(d => d.UserId)
    .FirstAsync();

        await notificationDispatcher.SendQrShareUpdatedAsync(
            qrShare.Id,
            userId,
            doctorUserId
        );

        return ResponseHelper.Response(
            data: qrShare.Id.ToString(),
            succeeded: true,
            message: SuccessMessages.Created("QR Share"),
            errors: null,
            statusCode: 201
        );
    }

    public async Task<Response<QrShareResponse>> GetByIdAsync(Guid id)
    {
        QrShareResponse? qrShare = await qrShareRepository.FirstOrDefaultAsync(
            r => r.Id == id,
            r => new QrShareResponse
            {
                Id = r.Id,
                DoctorName = r.DoctorProfile != null ? r.DoctorProfile.User.FirstName + " " + r.DoctorProfile.User.LastName : null,
                ExpiresAt = r.ExpiresAt,
                CreatedAt = r.CreatedAt,
                IsUsed = r.IsUsed,
            }
        );

        if (qrShare == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("QR Share"));
        }

        return ResponseHelper.Response(
                qrShare,
                true,
                SuccessMessages.RETRIEVED,
                null,
                (int)HttpStatusCode.OK
            );
    }

    public async Task<Response<List<QrShareResponse>>> GetByPatientAsync(int userId)
    {
        bool userExists = await userRepository.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        PatientProfile? patient = await patientProfileRepository.FirstOrDefaultAsync(p => p.UserId == userId);
        if (patient == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient"));
        }

        List<QrShareResponse> qrShares =
        (await qrShareRepository.GetListAsync(
            r => r.PatientId == patient.Id,
            r => new QrShareResponse
            {
                Id = r.Id,
                DoctorName = r.DoctorProfile != null ? r.DoctorProfile.User.FirstName + " " + r.DoctorProfile.User.LastName : null,
                ExpiresAt = r.ExpiresAt,
                CreatedAt = r.CreatedAt,
                IsUsed = r.IsUsed,
            }
        ))
        .OrderBy(x => x.CreatedAt)
        .ToList();

        return ResponseHelper.Response(
            data: qrShares,
            succeeded: true,
            message: SuccessMessages.RETRIEVED,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string>> DeleteAsync(Guid id)
    {
        QrShare? qrShare = await qrShareRepository.FirstOrDefaultAsync(x => x.Id == id);

        if (qrShare == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("QR Share"));
        }

        int patientUserId = await patientProfileRepository
    .Query()
    .Where(p => p.Id == qrShare.PatientId)
    .Select(p => p.UserId)
    .FirstAsync();

        int doctorUserId = await doctorProfileRepository
            .Query()
            .Where(d => d.Id == qrShare.DoctorId)
            .Select(d => d.UserId)
            .FirstAsync();

        qrShareRepository.Delete(qrShare);
        await qrShareRepository.SaveChangesAsync();

        await notificationDispatcher.SendQrShareUpdatedAsync(
    qrShare.Id,
    patientUserId,
    doctorUserId
);

        return ResponseHelper.Response<string>(
            null,
            true,
            SuccessMessages.Deleted("QR Share"),
            null,
            (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<List<QrShareByDoctorResponse>>> GetByDoctorAsync(int userId)
    {
        bool userExists = await userRepository.AnyAsync(u => u.Id == userId);
        if (!userExists)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        DoctorProfile? doctor = await doctorProfileRepository.FirstOrDefaultAsync(d => d.UserId == userId);
        if (doctor == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Doctor"));
        }

        List<QrShareByDoctorResponse> qrShares =
        (await qrShareRepository.GetListAsync(
            r => r.DoctorId == doctor.Id && !r.IsUsed && r.ExpiresAt > DateTime.UtcNow,
            r => new QrShareByDoctorResponse
            {
                Id = r.Id,
                PatientName = r.PatientProfile != null ? r.PatientProfile.User.FirstName + " " + r.PatientProfile.User.LastName : null,
                ExpiresAt = r.ExpiresAt,
                CreatedAt = r.CreatedAt,
            }
        ))
        .OrderBy(x => x.CreatedAt)
        .ToList();

        return ResponseHelper.Response(
            data: qrShares,
            succeeded: true,
            message: SuccessMessages.RETRIEVED,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<byte[]> GetQrImageByIdAsync(Guid qrShareId)
    {

        QrShare? qrShare = await qrShareRepository.FirstOrDefaultAsync(
            x => x.Id == qrShareId
        );

        if (qrShare == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("QR Share"));
        }

        if (qrShare.ExpiresAt <= DateTime.UtcNow)
        {
            throw new ArgumentException(ErrorMessages.QR_SHARE_EXPIRED);
        }

        string? baseUrl = configuration["App:BaseUrl"];
        string qrUrl = $"{baseUrl}/qr/view?token={Uri.EscapeDataString(qrShare.AccessToken)}";

        // string qrUrl = $"google.com";

        using QRCodeGenerator qrGenerator = new QRCodeGenerator();
        using QRCodeData qrCodeData = qrGenerator.CreateQrCode(
            qrUrl,
            QRCodeGenerator.ECCLevel.Q
        );
        using PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

        byte[] qrImageBytes = qrCode.GetGraphic(20);

        return qrImageBytes;
    }

    public async Task<string> GetQrTokenAsync(Guid qrShareId)
    {
        QrShare? qrShare = await qrShareRepository.FirstOrDefaultAsync(
                x => x.Id == qrShareId
            );

        if (qrShare == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("QR Share"));
        }

        if (qrShare.ExpiresAt <= DateTime.UtcNow)
        {
            throw new ArgumentException(ErrorMessages.QR_SHARE_EXPIRED);
        }

        return qrShare.AccessToken;
    }

    public async Task<Response<PatientQrAccessResponse>> GetPatientAccessByQrTokenAsync(
        string token,
        int doctorUserId
    )
    {
        QrShare? qrShare = await qrShareRepository
           .Query()
           .Include(q => q.PatientProfile)
               .ThenInclude(p => p.User)
           .Include(q => q.PatientProfile)
               .ThenInclude(p => p.MedicalTimelines)
                   .ThenInclude(mt => mt.Documents)
           .Include(q => q.PatientProfile)
               .ThenInclude(p => p.MedicalTimelines)
                   .ThenInclude(mt => mt.DoctorProfile)
                       .ThenInclude(d => d.User)
           .Include(q => q.DoctorProfile)
           .FirstOrDefaultAsync(q => q.AccessToken == token);

        if (qrShare == null)
        {
            throw new ArgumentException("Invalid QR token");
        }

        if (qrShare.ExpiresAt <= DateTime.UtcNow)
        {
            throw new ArgumentException(ErrorMessages.QR_SHARE_EXPIRED);
        }

        if (qrShare.IsUsed)
        {
            throw new ArgumentException("QR token already used");
        }

        if (qrShare.DoctorProfile.UserId != doctorUserId)
        {
            throw new ArgumentException("Unauthorized doctor access");
        }

        qrShare.IsUsed = true;
        qrShare.UsedAt = DateTime.UtcNow;
        qrShareRepository.Update(qrShare);
        await qrShareRepository.SaveChangesAsync();

        await notificationDispatcher.SendQrShareUpdatedAsync(
    qrShare.Id,
    qrShare.PatientProfile.UserId,
    doctorUserId
);


        PatientProfile? patient = qrShare.PatientProfile
            ?? throw new ArgumentException("Patient profile not found");

        return ResponseHelper.Response(
            data: new PatientQrAccessResponse
            {
                PatientId = patient.Id,

                PatientProfile = new PatientProfileResponse
                {
                    DateOfBirth = patient.DateOfBirth,
                    Gender = patient.Gender,
                    GenderValue = patient.Gender.ToString(),
                    BloodGroup = patient.BloodGroup,
                    BloodGroupValue = patient.BloodGroup.ToString(),
                    Allergies = patient.Allergies,
                    ChronicCondition = patient.ChronicCondition,
                    EmergencyContactName = patient.EmergencyContactName,
                    EmergencyContactPhone = patient.EmergencyContactPhone
                },

                MedicalTimelines = patient.MedicalTimelines
                    .OrderByDescending(m => m.EventDate)
                    .Select(m => new MedicalTimelineResponse
                    {
                        Id = m.Id,
                        PatientId = m.PatientId,
                        DoctorProfileId = m.DoctorProfileId,
                        DoctorProfileName = m.DoctorProfile != null
                            ? $"{m.DoctorProfile.User.FirstName} {m.DoctorProfile.User.LastName}"
                            : null,
                        DoctorName = m.DoctorName,
                        CheckupTypeId = m.CheckupType,
                        CheckupType = m.CheckupType.ToString(),
                        EventDate = m.EventDate,
                        Notes = m.Notes,
                        CreatedAt = m.CreatedAt,
                        UpdatedAt = m.UpdatedAt,
                        DocumentResponses = m.Documents.Select(d => new DocumentResponse
                        {
                            Id = d.Id,
                            FileName = d.FileName,
                            FileUrl = d.FileUrl
                        }).ToList()
                    })
                    .ToList()
            },
            succeeded: true,
            message: SuccessMessages.RETRIEVED,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }


}