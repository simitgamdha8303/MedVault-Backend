using MedVault.Services.IServices;
using MedVault.Services.Services;

namespace MedVault.Web.Extension;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddApplicationServices(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddScoped<IUserService, UserService>();

        return builder;
    }
}
