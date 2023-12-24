using DB;

namespace API;

public class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var config = builder.Configuration.Parse();
        Upgrade.SqlDatabase(config.DbConnectionString);
        builder.Services.Configure(config);
        var app = builder.Build();
        app.Configure();
        app.Run();
    }
}