namespace Library.Generator.Nats.Producer;

/// <summary>
/// Интерфейс для публикации сообщений в NATS JetStream.
/// Обеспечивает отправку DTO объектов в брокер для последующей обработки консьюмерами.
/// </summary>
public interface INatsProducer
{
    /// <summary>
    /// Публикует сообщение в NATS JetStream по указанному subject.
    /// </summary>
    /// <typeparam name="T">Тип отправляемого DTO сообщения (BookTypeCreateUpdateDto, AuthorCreateUpdateDto и т.д.)</typeparam>
    /// <param name="subject">Subject потока (например: "library.authors", "library.books", "library.issues").</param>
    /// <param name="payload">DTO объект для публикации в брокер.</param>
    /// <param name="cancellationToken">Токен отмены для прерывания операции публикации.</param>
    /// <returns>Task, завершающийся после публикации сообщения в брокер.</returns>
    public Task PublishAsync<T>(
        string subject,
        T payload,
        CancellationToken cancellationToken = default);
}
