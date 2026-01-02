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

        return app;
    }
}
