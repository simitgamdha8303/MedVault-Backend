using System.Net;
using System.Text.RegularExpressions;
using MedVault.Common.Helper;
using MedVault.Common.Messages;
using MedVault.Common.Response;
using MedVault.Data.IRepositories;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;
using MedVault.Services.IServices;
using MedVault.Utilities.EmailServices;
using MedVault.Utilities.Validations;
using Microsoft.AspNetCore.Identity;

namespace MedVault.Services.Services;

public class AuthService(
    IUserRepository userRepository,
    JwtService jwt,
    IOtpRepository otpRepository,
    IEmailService emailService
    ) : IAuthService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly JwtService _jwt = jwt;
    private readonly IOtpRepository _otpRepository = otpRepository;
    private readonly IEmailService _emailService = emailService;
    private readonly PasswordHasher<User> _passwordHasher = new();

    public async Task<Response<LoginResponse?>> LoginUserAsync(LoginRequest loginRequest)
    {
        string identifier = loginRequest.Identifier.Trim();

        bool isEmail = identifier.Contains("@");
        bool isPhone = Regex.IsMatch(identifier, @"^[6-9]\d{9}$");

        if (!isEmail && !isPhone)
        {
            throw new ArgumentException(ErrorMessages.InvalidEmailOrPhone);
        }

        // is user exists
        User? user = await _userRepository.FirstOrDefaultAsync(u => u.Email == identifier || u.Mobile == identifier);

        if (user == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            loginRequest.Password
        );

        if (result != PasswordVerificationResult.Success)
        {
            throw new UnauthorizedAccessException(ErrorMessages.Invalid("Password"));
        }

        //OTP
        if (user.TwoFactorEnabled || user.IsVerified == false)
        {
            List<OtpVerification> otpEntry = (await _otpRepository.FindAsync(o => o.UserId == user.Id)).ToList();

            if (otpEntry.Any())
            {
                _otpRepository.DeleteRange(otpEntry);
                await _otpRepository.SaveChangesAsync();
            }

            string otp = OtpGenerator.GenerateOtp();

            OtpVerification? otpEntity = new OtpVerification
            {
                UserId = user.Id,
                OtpHash = _passwordHasher.HashPassword(user, otp),
                ExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };

            await _otpRepository.AddAsync(otpEntity);

            //Send OTP via Email ( sms is pending )
            await _emailService.SendOtpAsync(user.Email, otp);

            return ResponseHelper.Response<LoginResponse?>(
                 data: new LoginResponse
                 {
                     RequiresOtp = true,
                     UserId = user.Id,
                     Token = null
                 },
                succeeded: true,
                message: SuccessMessages.OtpSent,
                errors: null,
                statusCode: (int)HttpStatusCode.Accepted
            );
        }

        // login (No OTP)
        string token = _jwt.GenerateToken(user);

        return ResponseHelper.Response<LoginResponse?>(
            data: new LoginResponse
            {
                RequiresOtp = false,
                Token = token,
                UserId = null
            },
            succeeded: true,
            message: SuccessMessages.LoginSuccess,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string?>> VerifyOtpAsync(VerifyOtpRequest request)
    {
        OtpVerification? otpEntry = await _otpRepository
            .FirstOrDefaultAsync(o =>
                o.UserId == request.UserId &&
                o.ExpiresAt > DateTime.UtcNow);

        if (otpEntry == null)
        {
            throw new ArgumentException(ErrorMessages.Invalid("OTP"));
        }

        User? user = await _userRepository.GetByIdAsync(request.UserId);

        PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(
            user,
            otpEntry.OtpHash,
            request.Otp
        );

        if (result != PasswordVerificationResult.Success)
        {
            throw new UnauthorizedAccessException(ErrorMessages.Invalid("OTP"));
        }

        user.IsVerified = true;

        _otpRepository.Delete(otpEntry);
        await _otpRepository.SaveChangesAsync();

        string token = _jwt.GenerateToken(user);

        return ResponseHelper.Response<string?>(
            data: token,
            succeeded: true,
            message: SuccessMessages.OtpVerified,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<string?>> ResendOtpAsync(ResendOtpRequest request)
    {
        User? user = await _userRepository.GetByIdAsync(request.UserId);

        if (user == null)
        {
            throw new ArgumentException(ErrorMessages.NotFound("User"));
        }

        // remove old OTPs
        List<OtpVerification> oldOtps =
            (await _otpRepository.FindAsync(o => o.UserId == user.Id)).ToList();

        if (oldOtps.Any())
        {
            _otpRepository.DeleteRange(oldOtps);
            await _otpRepository.SaveChangesAsync();
        }

        string otp = OtpGenerator.GenerateOtp();

        OtpVerification otpEntity = new()
        {
            UserId = user.Id,
            OtpHash = _passwordHasher.HashPassword(user, otp),
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };

        await _otpRepository.AddAsync(otpEntity);

        await _emailService.SendOtpAsync(user.Email, otp);

        return ResponseHelper.Response<string?>(
            data: null,
            succeeded: true,
            message: SuccessMessages.OtpSent,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }


}
