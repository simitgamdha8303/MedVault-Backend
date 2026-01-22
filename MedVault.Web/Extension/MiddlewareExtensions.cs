using Hangfire;
using MedVault.Infrastructure.Hubs;

using MedVault.Web.Middlewares;

namespace MedVault.Web.Extension;

public static class MiddlewareExtensions
{
    public static WebApplication UseApplicationMiddleware(
        this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseCors("AngularPolicy");
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.MapHub<NotificationHub>("/hubs/notifications");

        return app;
    }
}
