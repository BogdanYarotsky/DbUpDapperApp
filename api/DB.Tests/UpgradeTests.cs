using System.Runtime.CompilerServices;
using System.Text;
using Dapper;
using DbUp;
using Microsoft.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DB.Tests;

[TestClass]
public class UpgradeTests
{
    private static readonly string _connectionString = new SqlConnectionStringBuilder
    {
        UserID = "SA",
        Password = "<YourStrong@Passw0rd>",
        TrustServerCertificate = true,
        InitialCatalog = "Test",
        ConnectTimeout = 3,
        CommandTimeout = 3,
    }.ToString();

    private static SqlConnection NewConnection() 
        => new(_connectionString);

    [TestInitialize]
    public async Task EnsureEmptyDatabase()
    {
        EnsureDatabase.For.SqlDatabase(_connectionString);
        await using var conn = NewConnection();
        var tableNames = await conn.QueryAsync<string>(@"
            SELECT name 
            FROM sys.tables
            WHERE type = 'U';
        ");

        var sb = new StringBuilder();
        sb.AppendLine("EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT all';");
        foreach (var table in tableNames)
            sb.AppendLine($"DROP TABLE [{table}];");

        await conn.ExecuteAsync(sb.ToString());
    }

    [TestMethod]
    public void AllMigrationsApplySuccessfully()
    {
        Upgrade.SqlDatabase(_connectionString);
    }

    [TestMethod]
    public async Task TimeSlotTableIsCreated()
    {
        Upgrade.SqlDatabase(_connectionString, 1);
        await using var conn = new SqlConnection(_connectionString);
        var tablesCount = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM sys.tables WHERE name = 'TimeSlot'");
        Assert.AreEqual(1, tablesCount);
    }
}