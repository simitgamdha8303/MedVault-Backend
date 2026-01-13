using System.Data;
using Dapper;
using MedVault.Common.Response;
using MedVault.Data.IRepositories;
using MedVault.Models.Dtos.RequestDtos;
using MedVault.Models.Dtos.ResponseDtos;
using MedVault.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace MedVault.Data.Repositories;

public class DoctorProfileRepository : GenericRepository<DoctorProfile>, IDoctorProfileRepository
{
    private readonly string _connectionString;
    public DoctorProfileRepository(ApplicationDbContext context, IConfiguration configuration) : base(context)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public async Task<List<DoctorListResponse>> GetAllByFnAsync()
    {
        await using NpgsqlConnection? connection = new NpgsqlConnection(_connectionString);

        const string sql = "SELECT * FROM get_all_doctors()";
        IEnumerable<DoctorListResponse> list = await connection.QueryAsync<DoctorListResponse>(sql);

        return list.ToList();
    }

    public async Task<int> CreateHospitalBySpAsync(HospitalCreateRequest request)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        var parameters = new DynamicParameters();
        parameters.Add("p_name", request.Name);
        parameters.Add(
            "p_hospital_id",
            dbType: DbType.Int32,
            direction: ParameterDirection.Output
        );

        await connection.ExecuteAsync(
            "CALL add_hospital(@p_name::text)",
            parameters
        );

        return parameters.Get<int>("p_hospital_id");
    }




    public async Task<List<HospitalResponse>> GetAllHospitalByFnAsync()
    {
        await using NpgsqlConnection? connection = new NpgsqlConnection(_connectionString);

        List<HospitalResponse>? hospitals = (await connection.QueryAsync<HospitalResponse>(
            "SELECT * FROM public.get_hospitals()"
        )).ToList();

        return hospitals;
    }

}