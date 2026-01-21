using System.ComponentModel.DataAnnotations;
using MedVault.Common.Messages;

namespace MedVault.Models.Dtos.RequestDtos;

public class ResendOtpRequest
{
    [Required(ErrorMessage = ValidationMessages.USERID_REQUIRED)]

    public int UserId { get; set; }
}
