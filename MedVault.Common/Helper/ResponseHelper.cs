using MedVault.Common.Response;

namespace MedVault.Common.Helper;

public class ResponseHelper
{
    
    public static Response<T> Response<T>(T? data, bool succeeded, string? message, string[]? errors, int statusCode)
    {
        return new Response<T>
        {
            Data = data,
            Succeeded = succeeded,
            Message = message,
            Errors = errors,
            StatusCode = statusCode
        };
    }
}
