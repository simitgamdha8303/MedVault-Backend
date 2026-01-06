using AutoMapper;
using MedVault.Models.Entities;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Dtos.RequestDtos;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UserRequest, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

        CreateMap<DoctorProfileRequest, DoctorProfile>();

        CreateMap<DoctorProfile, DoctorProfileResponse>();
        
         CreateMap<PatientProfileRequest, PatientProfile>();
         
        CreateMap<PatientProfile, PatientProfileResponse>();

    }
}
