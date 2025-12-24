using Library.Api.Host.Middleware;
using Library.Api.Host.Services;
using Library.Application.Extensions;
using Library.Infrastructure.MongoEf.Extensions;
using Library.Infrastructure.Nats;
using Microsoft.OpenApi.Models;
using NATS.Client.Core;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// -------------------------
// MongoDB connection string
// -------------------------
var mongoConnectionString = builder.Configuration.GetConnectionString("mongodb")
    ?? "mongodb://mongodb:27017";

Console.WriteLine($"[API] MONGO CS from config: '{mongoConnectionString}'");

// Логирование
builder.Services.AddLogging(configure =>
{
    configure.ClearProviders();
    configure.AddConsole();
    configure.SetMinimumLevel(LogLevel.Information);
});

// MongoDB
builder.Services
    .AddMongoDbContext(mongoConnectionString, "LibraryDb")
    .AddRepositories()
    .AddApplicationServices()
    .AddAutoMapperConfiguration();

// -------------------------
// NATS connection string
// -------------------------
var natsConnectionString = builder.Configuration.GetConnectionString("nats")
    ?? "nats://nats:4222";

Console.WriteLine($"[API] NATS CS from config: '{natsConnectionString}'");

builder.Services.AddSingleton<INatsConnection>(_ =>
    new NatsConnection(new NatsOpts { Url = natsConnectionString }));

builder.Services.AddSingleton<INatsConsumer, NatsConsumer>();
builder.Services.AddHostedService<NatsConsumerService>();

// -------------------------
// CORS
// -------------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// API + Swagger
builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Library Management API",
        Version = "v1",
        Description = "REST API для управления библиотечной системой."
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        c.IncludeXmlComments(xmlPath);
    }
});

// Pipeline
var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// CORS middleware (ВАЖНО: перед UseAuthorization)
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseAuthorization();
app.MapControllers();

app.Run();
