using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// MongoDB + Mongo Express
var mongo = builder.AddMongoDB("mongodb")
    .WithMongoExpress();

var mongoDb = mongo.AddDatabase("LibraryDb");

// NATS
var nats = builder
    .AddNats("nats")
    .WithJetStream();

// NATS NUI
builder
    .AddContainer("nats-nui", "ghcr.io/nats-nui/nui:edge")
    .WithHttpEndpoint(name: "http", port: 8080, targetPort: 31311)
    .WithVolume("nats-nui-db", "/db")
    .WithVolume("nats-nui-proto", "/proto-schemas")
    .WaitFor(nats);

// API
builder.AddProject<Library_Api_Host>("api")
    .WithReference(mongoDb)
    .WithReference(nats)
    .WithExternalHttpEndpoints()
    .WaitFor(mongoDb);

// Generator
builder.AddProject<Library_Generator_Nats>("generator")
    .WithReference(nats)
    .WaitFor(nats);

builder.Build().Run();
