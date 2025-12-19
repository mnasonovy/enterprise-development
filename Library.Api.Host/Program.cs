using Library.Api.Host.Middleware;
using Library.Application.Extensions;
using Library.Infrastructure.MongoEf.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(configure =>
{
    configure.ClearProviders();
    configure.AddConsole();
    configure.SetMinimumLevel(LogLevel.Information);
});

var mongoConnectionString = builder.Configuration.GetConnectionString("mongodb")
    ?? throw new InvalidOperationException("MongoDB connection string 'mongodb' не найдена в appsettings.json");

builder.Services
    .AddMongoDbContext(mongoConnectionString, "library")
    .AddRepositories()
    .AddApplicationServices()
    .AddAutoMapperConfiguration();

builder.Services
    .AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.SuppressModelStateInvalidFilter = true;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Library Management API",
        Version = "v1",
        Description = "REST API для управления библиотекой",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Library Support",
            Url = new Uri("https://localhost:7000")
        }
    });

    var xmlFile = Path.Combine(AppContext.BaseDirectory, "Library.Api.Host.xml");
    if (File.Exists(xmlFile))
        c.IncludeXmlComments(xmlFile);
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Library API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
