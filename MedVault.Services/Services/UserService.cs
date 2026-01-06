using System.Net;
using AutoMapper;
using MedVault.Common.Helper;
using MedVault.Common.Messages;
using MedVault.Common.Response;
using MedVault.Data.IRepositories;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Entities;
using MedVault.Services.IServices;
using Microsoft.AspNetCore.Identity;


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

}
