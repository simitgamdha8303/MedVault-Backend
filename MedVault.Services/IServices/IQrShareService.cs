using MedVault.Common.Response;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;

namespace MedVault.Services.IServices;

public interface IQrShareService
{
 Task<Response<GenerateQrResponseDto>> GenerateAsync(int userId,GenerateQrRequestDto request);
}