using Microsoft.Extensions.Logging;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using System.Text;
using System.Text.Json;

namespace Library.Generator.Nats.Producer;

/// <summary>
/// Реализация публикации сообщений в NATS JetStream.
/// Сериализует DTO в JSON и публикует в указанный subject с автоматической инициализацией потока.
/// </summary>
public sealed class NatsProducer(
    INatsConnection connection,
    ILogger<NatsProducer> logger) : INatsProducer
{
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = null,
        WriteIndented = false
    };

    private bool _streamInitialized;
    private readonly object _lockObj = new();

    /// <summary>
    /// Публикует DTO сообщение в NATS JetStream.
    /// Автоматически инициализирует поток при первом использовании.
    /// </summary>
    public async Task PublishAsync<T>(
        string subject,
        T payload,
        CancellationToken cancellationToken = default)
    {
        var js = new NatsJSContext(connection);

        // Инициализируем поток один раз (потокобезопасно)
        if (!_streamInitialized)
        {
            lock (_lockObj)
            {
                if (!_streamInitialized)
                {
                    InitializeStreamAsync(js, subject, cancellationToken).GetAwaiter().GetResult();
                    _streamInitialized = true;
                }
            }
        }

        var json = JsonSerializer.Serialize(payload, _jsonOptions);
        var bytes = Encoding.UTF8.GetBytes(json);

        await js.PublishAsync(
            subject,
            bytes,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Published {Type} to {Subject}: {Json}",
            typeof(T).Name,
            subject,
            json);
    }

    /// <summary>
    /// Инициализирует JetStream поток для subject если он еще не существует.
    /// </summary>
    private static async Task InitializeStreamAsync(
        NatsJSContext js,
        string subject,
        CancellationToken cancellationToken)
    {
        var streamName = subject.Replace('.', '_');
        var streamConfig = new StreamConfig
        {
            Name = streamName,
            Subjects = [subject],
            MaxAge = TimeSpan.FromHours(24)
        };

        try
        {
            await js.GetStreamAsync(streamName, cancellationToken: cancellationToken);
        }
        catch (NatsJSApiException)
        {
            try
            {
                await js.CreateStreamAsync(streamConfig, cancellationToken: cancellationToken);
            }
            catch (NatsJSApiException ex)
            {
                throw new InvalidOperationException(
                    $"Failed to create JetStream stream '{streamName}'",
                    ex);
            }
        }
    }
}
