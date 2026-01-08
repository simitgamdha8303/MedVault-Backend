using AutoMapper;
using MedVault.Models.Entities;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Dtos.RequestDtos;

public class MedicalTimelineProfile : Profile
{
    public MedicalTimelineProfile()
    {
        CreateMap<MedicalTimelineRequest, MedicalTimeline>();
        CreateMap<MedicalTimeline, MedicalTimelineResponse>();
    }
}
