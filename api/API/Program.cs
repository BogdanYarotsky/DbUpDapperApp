using API;

var builder = WebApplication.CreateSlimBuilder(args);
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Add(AppJsonSerializerContext.Default);
});
var app = builder.Build();

var sampleTodos = new Todo[] {
    new(1, "Walk the dog", new TimeRange(new DateTime(), new DateTime())),
    new(2, "Do the dishes", new TimeRange(new DateTime(), new DateTime())),
    new(3, "Do the laundry", new TimeRange(new DateTime(), new DateTime())),
    new(4, "Clean the bathroom", new TimeRange(new DateTime(), new DateTime())),
    new(5, "Clean the car", new TimeRange(new DateTime(), new DateTime()))
};

var todosApi = app.MapGroup("/todos");
todosApi.MapGet("/", () => sampleTodos);
todosApi.MapGet("/{id}", (int id) =>
    sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
        ? Results.Ok(todo)
        : Results.NotFound());

app.Run();
public record Todo(int Id, string? Title, TimeRange TimeRange);