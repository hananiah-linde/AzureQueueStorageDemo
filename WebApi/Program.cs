var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.UseHttpsRedirection();

app.MapGet("/signup", () =>
    {
        return Results.Ok();
    })
    .WithName("Signup");

app.Run();