using API;
using API.Domain;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(AppJsonSerializerContext.Default);
});
builder.Services.AddCors();
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

app.UseCors(p => p.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:5173"));
app.Run();