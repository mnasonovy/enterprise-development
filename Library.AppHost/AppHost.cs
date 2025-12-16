using Projects;

var builder = DistributedApplication.CreateBuilder(args);

// MongoDB контейнер
var mongo = builder.AddMongoDB("mongodb")
    .WithMongoExpress();

// Явно описываем базу
var mongodb = mongo.AddDatabase("library");

// API сервис — ВАЖНО: указать тип проекта
var api = builder.AddProject<Projects.Library_Api_Host>("api")
    .WithReference(mongodb) // пробрасывает строку подключения в API
    .WaitFor(mongodb);

builder.Build().Run();
