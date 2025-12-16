using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// MongoDB контейнер
var mongo = builder.AddMongoDB("mongodb")
    .WithMongoExpress();

// База данных Library
var mongoDb = mongo.AddDatabase("library");

// API сервис с привязкой к базе
var api = builder.AddProject<Library_Api_Host>("api")
    .WithReference(mongoDb)
    .WaitFor(mongo);

builder.Build().Run();