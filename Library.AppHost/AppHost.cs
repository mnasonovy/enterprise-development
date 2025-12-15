var builder = DistributedApplication.CreateBuilder(args);

// MongoDB контейнер
var mongodb = builder.AddMongoDB("mongodb")
    .WithMongoExpress();

// API сервис
var api = builder.AddProject<Projects.Library_Api_Host>("api")
    .WithReference(mongodb)
    .WaitFor(mongodb);

builder.Build().Run();
