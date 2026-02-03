using System.Data;
using Dapper;
using MedVault.Data.IRepositories;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;
using MedVault.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace MedVault.Data.Repositories;

public class DoctorProfileRepository(ApplicationDbContext context, IConfiguration configuration) : GenericRepository<DoctorProfile>(context), IDoctorProfileRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection")!;

    public async Task<List<DoctorListResponse>> GetAllByFnAsync()
    {
        await using NpgsqlConnection? connection = new NpgsqlConnection(_connectionString);

        const string sql = "SELECT * FROM get_all_doctors()";
        IEnumerable<DoctorListResponse> list = await connection.QueryAsync<DoctorListResponse>(sql);

        return list.ToList();
    }


    public async Task<List<HospitalResponse>> GetAllHospitalByFnAsync()
    {
        await using NpgsqlConnection? connection = new NpgsqlConnection(_connectionString);

        const string sql = "SELECT * FROM public.get_hospitals()";
        List<HospitalResponse>? hospitals = (await connection.QueryAsync<HospitalResponse>(sql)).ToList();

        return hospitals;
    }

    public async Task<int> CreateHospitalAsync(string name)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        DynamicParameters? parameters = new DynamicParameters();
        parameters.Add("p_name", name, DbType.String, ParameterDirection.Input);
        parameters.Add("p_id", dbType: DbType.Int32, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "public.create_hospital",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return parameters.Get<int>("p_id");
    }


    public async Task<DoctorProfileResponse> CreateDoctorProfileAsync(DoctorProfileRequest request, int userId)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        DynamicParameters? parameters = new DynamicParameters();

        parameters.Add("p_user_id", userId);
        parameters.Add("p_hospital_id", request.HospitalId);
        parameters.Add("p_specialization", request.Specialization);
        parameters.Add("p_license_number", request.LicenseNumber);

        parameters.Add("p_hospital_id_out", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("p_specialization_out", dbType: DbType.String, direction: ParameterDirection.Output);
        parameters.Add("p_license_number_out", dbType: DbType.String, direction: ParameterDirection.Output);

        await connection.ExecuteAsync(
            "public.create_doctor_profile",
            parameters,
            commandType: CommandType.StoredProcedure
        );

        return new DoctorProfileResponse
        {
            HospitalId = parameters.Get<int>("p_hospital_id_out"),
            Specialization = parameters.Get<string>("p_specialization_out"),
            LicenseNumber = parameters.Get<string>("p_license_number_out"),
        };
    }

    public async Task<List<DoctorPatientListResponse>> GetPatientsByDoctorIdAsync(int doctorProfileId)
    {
        return await context.Appointments
       .Where(a =>
           a.DoctorId == doctorProfileId &&
           a.Status == AppointmentStatus.Completed
       )
       .GroupBy(a => new
       {
           a.PatientProfile.Id,
           a.PatientProfile.User.FirstName
       })
       .Select(g => new DoctorPatientListResponse
       {
           PatientId = g.Key.Id,
           PatientName = g.Key.FirstName,
           TotalVisits = g.Count()
       })
       .ToListAsync();
    }

}