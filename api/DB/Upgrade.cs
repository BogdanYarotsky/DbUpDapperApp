using DbUp;

namespace DB;

public static class Upgrade
{
    public static void SqlDatabase(string connectionString, int? targetMigrationNumber = null)
    {
        if (targetMigrationNumber is < 0)
            throw new ArgumentException("must be positive", nameof(targetMigrationNumber));

        EnsureDatabase.For.SqlDatabase(connectionString);

        var upgradeResult = DeployChanges
            .To.SqlDatabase(connectionString)
            .WithScriptsEmbeddedInAssembly(typeof(Upgrade).Assembly, script =>
            {
                var scriptFileName = script.Replace("DB.Migrations.", "");
                var migrationNumber = scriptFileName.Split("_").FirstOrDefault();
                if (!int.TryParse(migrationNumber, out var number))
                    throw new InvalidOperationException($"Found script name in invalid format: {scriptFileName}");

                if (targetMigrationNumber is null) return true;
                return number <= targetMigrationNumber;
            })
            .WithTransaction()
            .LogToConsole()
            .Build()
            .PerformUpgrade();

        if (!upgradeResult.Successful)
            throw upgradeResult.Error;
    }
}