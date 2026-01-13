using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;

namespace MedVault.Data.IRepositories;

public interface IDoctorProfileRepository : IGenericRepository<DoctorProfile>
{
    public Task<List<DoctorListResponse>> GetAllByFnAsync();

    // public Task<int> CreateHospitalBySpAsync(HospitalCreateRequest request);

    public Task<List<HospitalResponse>> GetAllHospitalByFnAsync();
}