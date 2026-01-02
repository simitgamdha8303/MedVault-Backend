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


namespace MedVault.Services.Services;

public class UserService(IUserRepository userRepository, IMapper mapper) : IUserService
{

    private readonly IUserRepository _userRepository = userRepository;
    private readonly PasswordHasher<User> _passwordHasher = new PasswordHasher<User>();
    private readonly IMapper _mapper = mapper;

    public async Task<Response<UserResponse?>> RegisterUserAsync(UserRequest userRequest)
    {

        // if user already exists
        User? existingUser =
            await _userRepository.FirstOrDefaultAsync(u => u.Email == userRequest.Email);

        if (existingUser != null)
        {
            throw new ArgumentException("User is Exists");
        }

        // Create new user
        User user = new User
        {
            Email = userRequest.Email,
            FirstName = userRequest.FirstName,
            MiddleName = userRequest.MiddleName,
            LastName = userRequest.LastName,
            Mobile = userRequest.Mobile,
        };

        user.PasswordHash = _passwordHasher.HashPassword(user, userRequest.Password);

        await _userRepository.AddAsync(user);

        UserResponse userResponse = _mapper.Map<UserResponse>(user);

        return ResponseHelper.Response<UserResponse?>(
            data: userResponse,
            succeeded: true,
            message: SuccessMessages.Registered("User"),
            errors: null,
            statusCode: (int)HttpStatusCode.Created
        );

    }

}
