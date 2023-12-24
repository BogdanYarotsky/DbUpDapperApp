using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace API;

public static class WebApp
{
    public static Configuration Parse(this IConfiguration configuration)
    {
        return new Configuration
        {
            AllowedCorsOrigins = configuration["AllowedCorsOrigins"]
                                 ?? throw new ArgumentException(
                                     "ClientUrl not found in app configuration"),

            DbConnectionString = configuration.GetConnectionString("PlanningDb")
                                 ?? throw new ArgumentException(
                                     "Can't start server without a db connection string")
        };
    }

    public static void Configure(this IServiceCollection services, Configuration configuration)
    {
        services.AddCors(cors =>
        {
            cors.AddDefaultPolicy(p =>
            {
                p.AllowAnyHeader().AllowAnyMethod().WithOrigins(configuration.AllowedCorsOrigins);
            });
        });

        services.AddScoped<IDbConnection>(_ => new SqlConnection(configuration.DbConnectionString));
    }

    public static void Configure(this WebApplication app)
    {
        app.UseCors();
        app.MapRoutes();
    }

    public static void MapRoutes(this IEndpointRouteBuilder app)
    {
        app.MapGet("/todos", async (IDbConnection dbConn) =>
        {
            try
            {
                var res = await dbConn.QuerySingleAsync("SELECT 1");
                return Results.Ok(res);
            }
            catch (Exception e)
            {
                return Results.Problem(new ProblemDetails
                {
                    Title = e.ToString()
                });
            }
        });
    }
}