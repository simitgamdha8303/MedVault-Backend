using CloudinaryDotNet;
using MedVault.Models;
using MedVault.Models.Dtos;
using MedVault.Services.IServices;
using MedVault.Services.Services;
using MedVault.Utilities.EmailServices;
using MedVault.Utilities.Validations;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace MedVault.Web.Extension;

public static class ServiceExtensions
{
    public static WebApplicationBuilder AddApplicationServices(
        this WebApplicationBuilder builder)
    {

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AngularPolicy", policy =>
            {
                policy
                    .WithOrigins("http://localhost:4200") // Angular app
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });


        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(
            options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "MedVault API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Enter 'Bearer' followed by your JWT token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },

                        Array.Empty<string>()
                    }
                });
            }
        );

        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAuthService, AuthService>();

        builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("Email"));

        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<IDoctorProfileService, DoctorProfileService>();
        builder.Services.AddScoped<IPatientProfileService, PatientProfileService>();
        builder.Services.AddScoped<IMedicalTimelineService, MedicalTimelineService>();
        builder.Services.AddScoped<ILookupService, LookupService>();

        builder.Services.AddScoped<JwtService>();

        builder.Services.Configure<CloudinarySettingsResponse>(
    builder.Configuration.GetSection("Cloudinary"));

        builder.Services.AddSingleton<Cloudinary>(sp =>
        {
            var settings = sp
                .GetRequiredService<IOptions<CloudinarySettingsResponse>>()
                .Value;

            var account = new Account(
                settings.CloudName,
                settings.ApiKey,
                settings.ApiSecret
            );

            return new Cloudinary(account);
        });



        return builder;
    }
}
