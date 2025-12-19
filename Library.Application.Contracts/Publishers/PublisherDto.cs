namespace Library.Application.Contracts.Publishers;

/// <summary>
/// DTO для передачи информации об издателе от сервера к клиенту.
/// Используется в GET запросах для отправки данных об издательстве в API ответе.
/// </summary>
public class PublisherDto
{
    /// <summary>
    /// Уникальный идентификатор издателя в системе.
    /// Это значение установлено клиентом при создании и хранится в БД.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название издательства (например: "Penguin Random House", "AST", "Eksmo").
    /// Отображается пользователям в интерфейсе приложения.
    /// Обязательное поле, максимум 100 символов.
    /// </summary>
    public required string Name { get; set; }
}
