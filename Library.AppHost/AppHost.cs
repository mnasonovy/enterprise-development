using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// MongoDB контейнер с Mongo Express для администрирования
var mongo = builder.AddMongoDB("mongodb")
    .WithMongoExpress();

// База данных Library в MongoDB
var mongoDb = mongo.AddDatabase("library");

// API сервис с привязкой к базе данных
var api = builder.AddProject<Projects.Library_Api_Host>("api")
    .WithReference(mongoDb)
    .WaitFor(mongoDb);

builder.Build().Run();
