using MedVault.Data;
using Microsoft.EntityFrameworkCore;

namespace MedVault.Web.Extension;

public static class DatabaseExtensions
{
    public static WebApplicationBuilder AddDatabase(
        this WebApplicationBuilder builder,
        IConfiguration configuration)
    {
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("MedVault.Data")
            )
        );

        return builder;
    }
}
