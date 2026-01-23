using System.Security.Cryptography;
using MedVault.Common.Helper;
using MedVault.Common.Messages;
using MedVault.Common.Response;
using MedVault.Data.IRepositories;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;
using MedVault.Services.IServices;
using Microsoft.Extensions.Configuration;

namespace MedVault.Services.Services;

public class QrShareService(IQrShareRepository qrShareRepository, IConfiguration configuration, IPatientProfileRepository patientProfileRepository) : IQrShareService
{
    public async Task<Response<GenerateQrResponseDto>> GenerateAsync(
         int userId,
         GenerateQrRequestDto generateQrRequestDto)
    {
        PatientProfile? patientProfile = await patientProfileRepository.FirstOrDefaultAsync(x => x.UserId == userId);

        if (patientProfile == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("Patient Profile"));
        }

        byte[]? tokenBytes = RandomNumberGenerator.GetBytes(64);
        string? accessToken = Convert.ToBase64String(tokenBytes);

        DateTime expiresAt = DateTime.UtcNow.AddMinutes(
            generateQrRequestDto.ExpiryMinutes > 0 ? generateQrRequestDto.ExpiryMinutes : 10);

        QrShare? qrShare = new QrShare
        {
            Id = Guid.NewGuid(),
            PatientId = patientProfile.Id,
            DoctorId = generateQrRequestDto.DoctorId,
            AccessToken = accessToken,
            ExpiresAt = expiresAt,
            IsActive = true,
            IsUsed = false,
            CreatedAt = DateTime.UtcNow
        };

        await qrShareRepository.AddAsync(qrShare);
        await qrShareRepository.SaveChangesAsync();

        string? baseUrl = configuration["App:BaseUrl"];
        string? qrUrl = $"{baseUrl}/qr/view?token={Uri.EscapeDataString(accessToken)}";

        return ResponseHelper.Response(
            data: new GenerateQrResponseDto
            {
                QrUrl = qrUrl,
                ExpiresAt = expiresAt
            },
            succeeded: true,
            message: SuccessMessages.Created("QR Share"),
            errors: null,
            statusCode: 201
        );
    }
}