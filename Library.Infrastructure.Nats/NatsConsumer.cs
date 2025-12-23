using Microsoft.Extensions.Logging;
using NATS.Client.Core;
using NATS.Client.JetStream;
using NATS.Client.JetStream.Models;
using System.Runtime.CompilerServices;

namespace Library.Infrastructure.Nats;

/// <summary>
/// Реализация потребления сообщений из NATS JetStream.
/// Автоматически создаёт поток и consumer при необходимости.
/// </summary>
public sealed class NatsConsumer(INatsConnection connection, ILogger<NatsConsumer> logger) : INatsConsumer
{
    /// <summary>
    /// Потребляет сообщения из NATS JetStream потока.
    /// Автоматически создаёт поток и durable consumer если они не существуют.
    /// </summary>
    public async IAsyncEnumerable<byte[]> ConsumeAsync(
        string streamName,
        string subject,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var js = new NatsJSContext(connection);

        // Проверяем / создаём поток
        var streamConfig = new StreamConfig
        {
            Name = streamName,
            Subjects = [subject],
            MaxAge = TimeSpan.FromHours(24)
        };

        try
        {
            await js.GetStreamAsync(streamName, cancellationToken: cancellationToken);
            logger.LogInformation("JetStream stream '{Stream}' already exists", streamName);
        }
        catch (NatsJSApiException)
        {
            logger.LogInformation("Creating JetStream stream '{Stream}' for subject '{Subject}'", streamName, subject);

            try
            {
                await js.CreateStreamAsync(streamConfig, cancellationToken: cancellationToken);
                logger.LogInformation("JetStream stream '{Stream}' created successfully", streamName);
            }
            catch (NatsJSApiException createEx)
            {
                logger.LogError(createEx, "Failed to create stream '{Stream}'", streamName);
                throw;
            }
        }

        // Создаём / обновляем durable consumer
        var consumerName = subject.Replace('.', '_');
        var consumerConfig = new ConsumerConfig
        {
            Name = consumerName,
            DurableName = consumerName,
            FilterSubject = subject,
            DeliverPolicy = ConsumerConfigDeliverPolicy.All,
            AckWait = TimeSpan.FromSeconds(30),
            MaxDeliver = 3
        };

        var consumer = await js.CreateOrUpdateConsumerAsync(
            streamName,
            consumerConfig,
            cancellationToken: cancellationToken);

        logger.LogInformation(
            "Consumer '{Consumer}' attached to stream '{Stream}' with subject '{Subject}'",
            consumerName,
            streamName,
            subject);

        // Читаем сообщения из потока
        var fetchOptions = new NatsJSFetchOpts
        {
            MaxMsgs = 100,
            Expires = TimeSpan.FromSeconds(1)
        };

        while (!cancellationToken.IsCancellationRequested)
        {
            await foreach (var msg in consumer.FetchAsync<byte[]>(
                opts: fetchOptions,
                cancellationToken: cancellationToken))
            {
                if (msg.Data is null || msg.Data.Length == 0)
                {
                    await msg.AckAsync(cancellationToken: cancellationToken);
                    continue;
                }

                yield return msg.Data;

                await msg.AckAsync(cancellationToken: cancellationToken);
            }
        }
    }
}
