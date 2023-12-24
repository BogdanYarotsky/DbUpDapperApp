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
        DataSource = "tcp:localhost,1433",
        Encrypt = false
    }.ToString();

    private static SqlConnection NewConnection() 
        => new(_connectionString);

    [TestInitialize]
    public async Task EnsureEmptyDatabase()
    {
        EnsureDatabase.For.SqlDatabase(_connectionString);
        await using var conn = NewConnection();
        await conn.ExecuteAsync(@"
            EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT all';
            EXEC sp_MSforeachtable 'DROP TABLE ?';
        ");
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