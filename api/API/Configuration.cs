namespace API;

public class Configuration
{
    public required string AllowedCorsOrigins { get; init; }
    public required string DbConnectionString { get; init; }
}