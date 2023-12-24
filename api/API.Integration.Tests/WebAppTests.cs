using System.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Data.SqlClient;

namespace API.Integration.Tests;

[TestClass]
public class WebAppTests
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

    private HttpClient _c;

    [TestInitialize]
    public void Initialize()
    {
        var configurationValues = new Dictionary<string, string?>
        {
            { "ConnectionStrings:PlanningDb", _connectionString },
            { "AllowedCorsOrigins", "http://localhost:88" },
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configurationValues)
            .Build();

        var a = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(host =>
            {
                host.UseConfiguration(configuration);
            });

        _c = a.CreateClient();
    }

    [TestMethod]
    public async Task TestMethod1()
    {
        var resp = await _c.GetAsync("todos");
        Assert.IsNotNull(resp);
    }
}