using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;

namespace MedVault.Services.IServices;

public interface IQrShareService
{
    Task<Response<string>> GenerateAsync(int userId, GenerateQrRequest GenerateQrRequest);
    Task<Response<List<QrShareResponse>>> GetByPatientAsync(int userId);
    Task<Response<List<QrShareResponse>>> GetByDoctorAsync(int userId);
    Task<Response<QrShareResponse>> GetByIdAsync(Guid id);
    Task<Response<string>> DeleteAsync(Guid id);

}