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
        builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
        builder.Services.AddScoped<IDoctorProfileRepository, DoctorProfileRepository>();
        builder.Services.AddScoped<IHospitalRepository, HospitalRepository>();
        builder.Services.AddScoped<IPatientProfileRepository, PatientProfileRepository>();
        builder.Services.AddScoped<IMedicalTimelineRepository, MedicalTimelineRepository>();
        builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
        builder.Services.AddScoped<IReminderRepository, ReminderRepository>();


        return builder;
    }
}
