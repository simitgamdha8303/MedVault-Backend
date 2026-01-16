using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;

namespace MedVault.Data.IRepositories;

public interface IDoctorProfileRepository : IGenericRepository<DoctorProfile>
{
    public Task<List<DoctorListResponse>> GetAllByFnAsync();

    public Task<List<HospitalResponse>> GetAllHospitalByFnAsync();

    public Task<int> CreateHospitalAsync(string name);

    public Task<DoctorProfileResponse> CreateDoctorProfileAsync(DoctorProfileRequest request, int userId);
}