namespace Library.Infrastructure.Nats;

/// <summary>
/// Интерфейс для потребления сообщений из NATS JetStream.
/// Обеспечивает подписку на subject и получение сообщений в виде байтовых массивов.
/// </summary>
public interface INatsConsumer
{
    /// <summary>
    /// Потребляет сообщения из NATS JetStream потока.
    /// </summary>
    /// <param name="streamName">Имя потока JetStream.</param>
    /// <param name="subject">Предмет для подписки (например: "library.authors", "library.books").</param>
    /// <param name="cancellationToken">Токен отмены для завершения потребления.</param>
    /// <returns>Асинхронный перечислитель байтовых массивов (сообщений).</returns>
    public IAsyncEnumerable<byte[]> ConsumeAsync(
        string streamName,
        string subject,
        CancellationToken cancellationToken = default);
}
