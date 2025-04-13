using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

app.UseCors("MyPolicy");

app.Use(async (context, next) =>
{
    if (context.Request.Headers.ContainsKey("Origin"))
    {
        context.Request.Headers.Origin = "https://cliente.macropampa.com";
    }
    await next();
});

await app.UseOcelot();

app.Run();
