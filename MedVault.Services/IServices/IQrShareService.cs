using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;

namespace MedVault.Services.IServices;

public interface IQrShareService
{
    Task<Response<string>> GenerateAsync(int userId, GenerateQrRequest GenerateQrRequest);
    Task<Response<List<QrShareResponse>>> GetByPatientAsync(int userId);
    Task<Response<List<QrShareByDoctorResponse>>> GetByDoctorAsync(int userId);
    Task<Response<QrShareResponse>> GetByIdAsync(Guid id);
    Task<Response<string>> DeleteAsync(Guid id);
    Task<byte[]> GetQrImageByIdAsync(Guid id);
    Task<string> GetQrTokenAsync(Guid qrShareId);
    Task<Response<PatientQrAccessResponse>> GetPatientAccessByQrTokenAsync(string token, int doctorUserId);

}