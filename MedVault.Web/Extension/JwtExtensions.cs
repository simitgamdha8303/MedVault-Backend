using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace MedVault.Web.Extension;

public static class JwtExtensions
{
    public static WebApplicationBuilder AddJwtServices(
        this WebApplicationBuilder builder)
    {
        // JWT Authentication 
        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var config = builder.Configuration;

            var jwtKey = config["Jwt:Key"];
            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                throw new InvalidOperationException("JWT Key is missing in configuration");
            }

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = config["Jwt:Issuer"],
                ValidAudience = config["Jwt:Audience"],

                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey)
                )
            };
        });


        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            options.AddPolicy("Doctor", policy => policy.RequireRole("Doctor"));
            options.AddPolicy("Patient", policy => policy.RequireRole("Patient"));

        });

        return builder;
    }
}
