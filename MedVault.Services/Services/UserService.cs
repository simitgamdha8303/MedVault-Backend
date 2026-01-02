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

    public async Task<Response<string>> RegisterUserAsync(UserRequest userRequest)
    {

        // if user already exists
        bool userExists = await _userRepository.AnyAsync(u => u.Email == userRequest.Email);

        if (userExists!)
        {
            throw new ArgumentException(ErrorMessages.AlreadyExists("User"));
        }

        // Create new user
        User user = _mapper.Map<User>(userRequest);

        // hash password
        user.PasswordHash = _passwordHasher.HashPassword(user, userRequest.Password);



        user.PasswordHash = _passwordHasher.HashPassword(user, userRequest.Password);

        await _userRepository.AddAsync(user);

        return ResponseHelper.Response<string>(
            data: null,
            succeeded: true,
            message: SuccessMessages.Registered("User"),
            errors: null,
            statusCode: (int)HttpStatusCode.Created
        );

    }

}
