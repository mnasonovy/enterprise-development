using Library.Generator.Nats.Producer;
using Library.Generator.Nats.Services;
using Library.Generator.Nats.Services.Senders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = Host.CreateApplicationBuilder(args);

// ============================================================================
// NATS JetStream
// ============================================================================
builder.AddNatsClient("nats");
builder.Services.AddScoped<INatsProducer, NatsProducer>();

// ============================================================================
// Отправители данных
// ============================================================================
builder.Services.AddScoped<BookTypeSender>();
builder.Services.AddScoped<AuthorSender>();
builder.Services.AddScoped<PublisherSender>();
builder.Services.AddScoped<ReaderSender>();
builder.Services.AddScoped<BookSender>();
builder.Services.AddScoped<IssueSender>();

// ============================================================================
// Генератор данных
// ============================================================================
builder.Services.AddHostedService<DataGeneratorService>();

var app = builder.Build();
await app.RunAsync();
