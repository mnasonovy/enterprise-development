using Projects;

/// <summary>
/// .NET Aspire AppHost - оркестрация контейнеризированного окружения разработки.
/// 
/// Отвечает за:
/// - Запуск MongoDB контейнера с Mongo Express для администрирования БД
/// - Создание базы данных Library в MongoDB
/// - Регистрацию и настройку API сервиса с зависимостями от MongoDB
/// 
/// При запуске создаёт:
/// 1. MongoDB контейнер с exposed портом для подключения
/// 2. Mongo Express Dashboard (UI для работы с БД)
/// 3. API сервис с автоматическим connection string в зависимости от MongoDB
/// 
/// Все компоненты взаимозависимы - API дожидается полной инициализации MongoDB перед стартом.
/// </summary>

var builder = DistributedApplication.CreateBuilder(args);

// MongoDB контейнер с Mongo Express UI для администрирования БД
var mongo = builder.AddMongoDB("mongodb")
    .WithMongoExpress();

// База данных Library, которая будет использоваться API сервисом
var mongoDb = mongo.AddDatabase("library");

// REST API сервис с привязкой к MongoDB базе данных
// WithReference автоматически передаёт connection string в конфигурацию
// WaitFor гарантирует, что MongoDB полностью инициализирована перед стартом API
_ = builder.AddProject<Library_Api_Host>("api")
    .WithReference(mongoDb)
    .WaitFor(mongoDb);

builder.Build().Run();