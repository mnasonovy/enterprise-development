using Microsoft.Extensions.DependencyInjection;

namespace Library.Infrastructure.Nats;

/// <summary>
/// Расширения для регистрации NATS инфраструктуры в DI контейнере.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Регистрирует NATS потребителя в DI контейнере.
    /// </summary>
    public static IServiceCollection AddNatsInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<INatsConsumer, NatsConsumer>();
        return services;
    }
}
