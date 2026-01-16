using Dapper;
using MedVault.Data.IRepositories;
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


    public async Task<List<HospitalResponse>> GetAllHospitalByFnAsync()
    {
        await using NpgsqlConnection? connection = new NpgsqlConnection(_connectionString);

        const string sql = "SELECT * FROM public.get_hospitals()";
        List<HospitalResponse>? hospitals = (await connection.QueryAsync<HospitalResponse>(sql)).ToList();

        return hospitals;
    }

}