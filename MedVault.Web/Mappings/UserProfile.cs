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

        // 🔹 USER UPDATE
        CreateMap<UpdateUserProfileRequest, User>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.Email, o => o.Ignore())
            .ForMember(d => d.PasswordHash, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UserRoles, o => o.Ignore())
            .ForMember(d => d.DoctorProfile, o => o.Ignore())
            .ForMember(d => d.PatientProfile, o => o.Ignore());

        // 🔹 DOCTOR UPDATE
        CreateMap<UpdateUserProfileRequest, DoctorProfile>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore());

        // 🔹 PATIENT UPDATE
        CreateMap<UpdateUserProfileRequest, PatientProfile>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.UserId, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore());


    }
}
