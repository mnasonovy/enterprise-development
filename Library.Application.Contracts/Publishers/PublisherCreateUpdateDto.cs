namespace Library.Application.Contracts.Publishers;

/// <summary>
/// DTO для создания и обновления издателя.
/// Используется в POST и PUT запросах для передачи данных об издательстве.
/// Содержит ID, устанавливаемый вручную при создании новой записи.
/// </summary>
public class PublisherCreateUpdateDto
{
    /// <summary>
    /// Уникальный идентификатор издателя.
    /// Устанавливается вручную при создании (обязателен и должен быть > 0).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название издательства (например: "Penguin Random House", "AST", "Eksmo").
    /// Обязательное поле, не может быть пусто или содержать только пробелы.
    /// </summary>
    public required string Name { get; set; }
}
