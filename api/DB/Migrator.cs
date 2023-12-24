using DbUp;

namespace DB;

public static class Migrator
{
    public static void Migrate(string connectionString)
    {
        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgradeEngine = DeployChanges
            .To.SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(Migrator).Assembly)
            .WithTransaction()
            .LogToConsole()
            .Build();

        var result = upgradeEngine.PerformUpgrade();
        if (!result.Successful)
        {
            throw result.Error;
        }
    }
}