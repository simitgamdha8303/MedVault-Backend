using AutoMapper;
using MedVault.Models.Entities;
using MedVault.Models.Dtos.ResponseDtos;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>();
    }
}
