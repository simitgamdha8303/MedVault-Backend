using System.Net;
using AutoMapper;
using MedVault.Common.Helper;
using MedVault.Common.Messages;
using MedVault.Common.Response;
using MedVault.Data.IRepositories;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;
using MedVault.Services.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace MedVault.Services.Services;

public class UserService(IUserRepository userRepository, IMapper mapper, IUserRoleRepository userRoleRepository) : IUserService
{
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    public async Task<Response<string>> RegisterUserAsync(UserRequest userRequest)
    {

        // if user already exists
        bool userExists = await userRepository.AnyAsync(u => u.Email == userRequest.Email);

        if (userExists)
        {
            throw new ArgumentException(ErrorMessages.AlreadyExists("User"));
        }

        bool mobileExists = await userRepository.AnyAsync(u => u.Mobile == userRequest.Mobile);

        if (mobileExists)
        {
            throw new ArgumentException(ErrorMessages.AlreadyExists("Mobile"));
        }

        // Create new user
        User user = mapper.Map<User>(userRequest);

        // hash password
        user.PasswordHash = _passwordHasher.HashPassword(user, userRequest.Password);

        user.CreatedAt = DateTime.UtcNow;

        await userRepository.AddAsync(user);

        // Assign user role
        UserRole userRole = new UserRole
        {
            UserId = user.Id,
            Role = userRequest.Role
        };

        await userRoleRepository.AddAsync(userRole);

        return ResponseHelper.Response<string>(
            data: null,
            succeeded: true,
            message: SuccessMessages.Registered("User"),
            errors: null,
            statusCode: (int)HttpStatusCode.Created
        );

    }

    public async Task<Response<UserProfileResponse>> GetMyProfileAsync(int userId)
    {
        User? user = await userRepository.Query()
            .Include(u => u.UserRoles)
            .Include(u => u.DoctorProfile)
                .ThenInclude(d => d.Hospital)
            .Include(u => u.PatientProfile)
            .FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return ResponseHelper.Response<UserProfileResponse>(
                data: null,
                succeeded: false,
                message: ErrorMessages.NotFound("User"),
                errors: null,
                statusCode: (int)HttpStatusCode.NotFound
            );
        }

        string? role = user.UserRoles.First().Role.ToString();

        UserProfileResponse? userProfileResponse = new UserProfileResponse
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Mobile = user.Mobile,
            TwoFactorEnabled = user.TwoFactorEnabled,
            Role = role,

            DoctorProfile = user.DoctorProfile == null ? null : new DoctorProfileResponse
            {
                Specialization = user.DoctorProfile.Specialization,
                LicenseNumber = user.DoctorProfile.LicenseNumber,
                HospitalName = user.DoctorProfile.Hospital.Name
            },

            PatientProfile = user.PatientProfile == null ? null : new PatientProfileResponse
            {
                DateOfBirth = user.PatientProfile.DateOfBirth,
                GenderValue = user.PatientProfile.Gender.ToString(),
                BloodGroupValue = user.PatientProfile.BloodGroup.ToString(),
                Allergies = user.PatientProfile.Allergies,
                ChronicCondition = user.PatientProfile.ChronicCondition,
                EmergencyContactName = user.PatientProfile.EmergencyContactName,
                EmergencyContactPhone = user.PatientProfile.EmergencyContactPhone
            }
        };

        return ResponseHelper.Response(
            data: userProfileResponse,
            succeeded: true,
            message: SuccessMessages.RETRIEVED,
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }

    public async Task<Response<bool>> UpdateTwoFactorAsync(int userId, bool enabled)
    {
        User? user = await userRepository.FirstOrDefaultAsync(u => u.Id == userId);

        if (user == null)
        {
            return ResponseHelper.Response(
                data: false,
                succeeded: false,
                message: ErrorMessages.NotFound("User"),
                errors: null,
                statusCode: (int)HttpStatusCode.NotFound
            );
        }

        user.TwoFactorEnabled = enabled;
        user.UpdatedAt = DateTime.UtcNow;

        userRepository.Update(user);
        await userRepository.SaveChangesAsync();

        return ResponseHelper.Response(
            data: enabled,
            succeeded: true,
            message: "Two-factor authentication updated",
            errors: null,
            statusCode: (int)HttpStatusCode.OK
        );
    }


}
