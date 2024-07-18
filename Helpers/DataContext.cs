namespace WebApi.Helpers;

using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

public class DataContext
{
    private DbSettings _dbSettings;

    public DataContext(IOptions<DbSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }

    public IDbConnection CreateConnection()
    {
        var connectionString = $"Server={_dbSettings.Server}; Database={_dbSettings.Database}; Uid={_dbSettings.UserId}; Pwd={_dbSettings.Password};TrustServerCertificate=True;";
        return new SqlConnection(connectionString);
    }

    public async Task Init()
    {
        await _initDatabase();
        await _initTables();
    }

    private async Task _initDatabase()
    {
        // create database if it doesn't exist
        var connectionString = $"Server={_dbSettings.Server}; Uid={_dbSettings.UserId}; Pwd={_dbSettings.Password};TrustServerCertificate=True";
        using var connection = new SqlConnection(connectionString);
        var sql = $"IF NOT EXISTS (SELECT * FROM sys.databases WHERE name ='{_dbSettings.Database}')" +
                  $"BEGIN CREATE DATABASE {_dbSettings.Database} END;";
        await connection.ExecuteAsync(sql);
    }

    private async Task _initTables()
    {
        // create tables if they don't exist
        using var connection = CreateConnection();
        await _initUsers();

        async Task _initUsers()
        {
            var sql = """
                IF NOT EXISTS(SELECT * FROM sys.tables WHERE name = 'Users')
                BEGIN
                CREATE TABLE Users (
                    Id INT NOT NULL IDENTITY(1,1),
                    Title VARCHAR(255),
                    FirstName VARCHAR(255),
                    LastName VARCHAR(255),
                    Email VARCHAR(255),
                    Role INT,
                    PasswordHash VARCHAR(255),
                    PRIMARY KEY (Id)
                );
                END
            """;
            await connection.ExecuteAsync(sql);
        }
    }
}