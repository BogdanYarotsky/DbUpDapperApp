using API;
using Dapper;
using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);
builder.AddClientCorsPolicy();
var app = builder.Build();

var sampleTodos = Enumerable
    .Range(0, 5)
    .Select(_ => new TimeSlot(new Guid(), new Guid(), new TimeRange()))
    .ToArray();

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", async () =>
{
    try
    {
        var cs = builder.Configuration.GetConnectionString("PlanningDb");
        await using var dbConn = new SqlConnection(cs);
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

todosApi.MapGet("/{id}", (Guid id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.UseCors();
app.Run();