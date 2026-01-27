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
using Microsoft.Extensions.Configuration;
using QRCoder;

namespace MedVault.Services.Services;

public class QrShareService(IQrShareRepository qrShareRepository,
 IConfiguration configuration,
 IPatientProfileRepository patientProfileRepository,
 IDoctorProfileRepository doctorProfileRepository,
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

        string? baseUrl = configuration["App:BaseUrl"];
        string? qrUrl = $"{baseUrl}/qr/view?token={Uri.EscapeDataString(accessToken)}";

        using QRCodeGenerator qrGenerator = new QRCodeGenerator();
        using QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrUrl, QRCodeGenerator.ECCLevel.Q);
        using PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);

        byte[] qrImageBytes = qrCode.GetGraphic(20);
        string qrImageBase64 = Convert.ToBase64String(qrImageBytes);

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

        qrShareRepository.Delete(qrShare);
        await qrShareRepository.SaveChangesAsync();

        return ResponseHelper.Response<string>(
            null,
            true,
            SuccessMessages.Deleted("QR Share"),
            null,
            (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<List<QrShareResponse>>> GetByDoctorAsync(int userId)
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

        List<QrShareResponse> qrShares =
        (await qrShareRepository.GetListAsync(
            r => r.DoctorId == doctor.Id,
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

}