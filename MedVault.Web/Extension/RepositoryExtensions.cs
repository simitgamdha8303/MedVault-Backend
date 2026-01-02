using MedVault.Data.IRepositories;
using MedVault.Data.Repositories;

namespace MedVault.Web.Extension;

public static class RepositoryExtensions
{
    public static WebApplicationBuilder AddRepositories(
        this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped(
            typeof(IGenericRepository<>),
            typeof(GenericRepository<>)
        );

        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IOtpRepository, OtpRepository>();


        return builder;
    }
}
