using AutoMapper;
using MedVault.Models.Entities;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Dtos.RequestDtos;

public class HospitalProfile : Profile
{
    public HospitalProfile()
    {
        CreateMap<Hospital, HospitalResponse>();

    }
}
