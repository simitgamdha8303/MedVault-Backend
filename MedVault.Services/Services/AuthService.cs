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
using MedVault.Utilities.EmailServices;
using MedVault.Utilities.Validations;
using Microsoft.AspNetCore.Identity;

namespace MedVault.Services.Services;

public class AuthService(
    IDoctorProfileRepository doctorProfileRepository,
    IPatientProfileRepository patientProfileRepository,
    IUserRepository userRepository,
    JwtService jwt,
    IUserRoleRepository userRoleRepository,
    IOtpRepository otpRepository,
    IEmailService emailService
    ) : IAuthService
{
    private readonly PasswordHasher<User> _passwordHasher = new();

    public async Task<Response<LoginResponse>> LoginUserAsync(LoginRequest loginRequest)
    {
        // is user exists
        User? user = await userRepository.FirstOrDefaultAsync(u => u.Email == loginRequest.Email);

        if (user == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        UserRole? userRole = await userRoleRepository.FirstOrDefaultAsync(ur => ur.UserId == user.Id && ur.Role == loginRequest.Role);

        if (userRole == null)
        {
            string roleName = Enum.GetName(typeof(Role), loginRequest.Role) ?? "Unknown";
            throw new ArgumentException(ErrorMessages.NotFound($"{roleName} profile"));
        }

        PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            loginRequest.Password
        );

        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            throw new UnauthorizedAccessException(ErrorMessages.Invalid("Password"));
        }

        //OTP
        if (user.TwoFactorEnabled || user.IsVerified == false)
        {
            List<OtpVerification> otpEntry = (await otpRepository.FindAsync(o => o.UserId == user.Id)).ToList();

            if (otpEntry.Any())
            {
                otpRepository.DeleteRange(otpEntry);
                await otpRepository.SaveChangesAsync();
            }

            string otp = OtpGenerator.GenerateOtp();

            OtpVerification? otpEntity = new OtpVerification
            {
                UserId = user.Id,
                OtpHash = _passwordHasher.HashPassword(user, otp),
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };

            await otpRepository.AddAsync(otpEntity);

            await emailService.SendOtpAsync(user.Email, otp);

            return ResponseHelper.Response(
                 data: new LoginResponse
                 {
                     RequiresOtp = true,
                     UserId = user.Id,
                     Token = null,
                     RequiresProfile = false
                 },
                succeeded: true,
                message: SuccessMessages.OTP_SENT,
                errors: null,
                statusCode: (int)HttpStatusCode.Accepted
            );
        }

        // login (No OTP)
        string token = jwt.GenerateToken(user, loginRequest.Role);

        bool isProfileComplete = false;

        if (userRole.Role == Role.Doctor)
        {
            isProfileComplete = await doctorProfileRepository.AnyAsync(u => u.UserId == user.Id);
        }
        else
        {
            isProfileComplete = await patientProfileRepository.AnyAsync(u => u.UserId == user.Id);
        }

        return ResponseHelper.Response(
            data: new LoginResponse
            {
                RequiresOtp = false,
                Token = token,
                UserId = null,
                RequiresProfile = isProfileComplete
            },
            succeeded: true,
            message: SuccessMessages.LOGIN_SUCCESS,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<OtpResponse>> VerifyOtpAsync(VerifyOtpRequest request)
    {
        OtpVerification? otpEntry = await otpRepository
            .FirstOrDefaultAsync(o =>
                o.UserId == request.UserId &&
                o.ExpiresAt > DateTime.UtcNow);

        if (otpEntry == null)
        {
            throw new ArgumentException(ErrorMessages.Invalid("OTP"));
        }

        User? user = await userRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        PasswordVerificationResult passwordVerificationResult = _passwordHasher.VerifyHashedPassword(
            user,
            otpEntry.OtpHash,
            request.Otp
        );

        if (passwordVerificationResult != PasswordVerificationResult.Success)
        {
            throw new UnauthorizedAccessException(ErrorMessages.Invalid("OTP"));
        }

        user.IsVerified = true;

        otpRepository.Delete(otpEntry);
        await otpRepository.SaveChangesAsync();

        string token = jwt.GenerateToken(user, request.Role);

        bool isProfileComplete = false;

        if (request.Role == Role.Doctor)
        {
            isProfileComplete = await doctorProfileRepository.AnyAsync(u => u.UserId == user.Id);
        }
        else
        {
            isProfileComplete = await patientProfileRepository.AnyAsync(u => u.UserId == user.Id);
        }

        return ResponseHelper.Response<OtpResponse>(
            data: new OtpResponse
            {
                Token = token,
                RequiresProfile = isProfileComplete
            },
            succeeded: true,
            message: SuccessMessages.OTP_VERIFIED,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string>> ResendOtpAsync(ResendOtpRequest request)
    {
        User? user = await userRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        // remove old OTPs
        List<OtpVerification> oldOtps =
            (await otpRepository.FindAsync(o => o.UserId == user.Id)).ToList();

        if (oldOtps.Any())
        {
            otpRepository.DeleteRange(oldOtps);
            await otpRepository.SaveChangesAsync();
        }

        string otp = OtpGenerator.GenerateOtp();

        OtpVerification otpEntity = new()
        {
            UserId = user.Id,
            OtpHash = _passwordHasher.HashPassword(user, otp),
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };

        await otpRepository.AddAsync(otpEntity);

        await emailService.SendOtpAsync(user.Email, otp);

        return ResponseHelper.Response<string>(
            data: null,
            succeeded: true,
            message: SuccessMessages.OTP_SENT,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }


}
