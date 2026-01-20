using AutoMapper;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Entities;

namespace MedVault.Models.Mappings;

public class ReminderProfile : Profile
{
    public ReminderProfile()
    {
        CreateMap<CreateReminderRequest, Reminder>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.PatientId, o => o.Ignore())
            .ForMember(d => d.IsActive, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.UpdatedAt, o => o.Ignore())
            .ForMember(d => d.PatientProfile, o => o.Ignore());

        CreateMap<UpdateReminderRequest, Reminder>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.PatientId, o => o.Ignore())
            .ForMember(d => d.CreatedAt, o => o.Ignore())
            .ForMember(d => d.PatientProfile, o => o.Ignore());
    }
}
