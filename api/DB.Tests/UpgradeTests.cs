using Dapper;
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

    [TestInitialize]
    public async Task EnsureDatabaseIsInCleanState()
    {
        await using var conn = new SqlConnection(_connectionString);
        await conn.ExecuteAsync("DROP DATABASE [Test]");
    }

    [TestMethod]
    public void ApplyingAllMigrationsDoesNotThrow()
    {
        Upgrade.SqlDatabase(_connectionString);
    }

    [TestMethod]
    public async Task TimeSlotTableIsCreated()
    {
        Upgrade.SqlDatabase(_connectionString, 1);
        await using var conn = new SqlConnection(_connectionString);
        var tablesCount = await conn.ExecuteScalarAsync<int>(
            "SELECT COUNT(*) FROM sys.databases WHERE name = N'TimeSlot'");
        Assert.AreEqual(1, tablesCount);
    }
}