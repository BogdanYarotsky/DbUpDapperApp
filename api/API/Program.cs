using Domain;

var builder = WebApplication.CreateBuilder(args);

var clientUrl = builder.Configuration["ClientUrl"] 
                ?? throw new ArgumentException(
                    "ClientUrl not found in app configuration");

builder.Services.AddCors(cors =>
{
    cors.AddDefaultPolicy(p =>
    {
        p.AllowAnyHeader().AllowAnyMethod().WithOrigins(clientUrl);
    });
});

var app = builder.Build();

var sampleTodos = new TimeSlot[] {
    new(new Guid(), new Guid(), new TimeRange(new(), new())),
    new(new Guid(), new Guid(), new TimeRange(new(), new())),
    new(new Guid(), new Guid(), new TimeRange(new(), new())),
    new(new Guid(), new Guid(), new TimeRange(new(), new())),
    new(new Guid(), new Guid(), new TimeRange(new(), new()))
};


var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id}", (Guid id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.UseCors();
app.Run();