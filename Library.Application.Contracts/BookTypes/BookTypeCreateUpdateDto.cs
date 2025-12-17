namespace Library.Application.Contracts.BookTypes;

/// <summary>
/// DTO для создания и обновления типа книги.
/// Используется в POST и PUT запросах для передачи данных о типе книги.
/// Содержит ID, устанавливаемый вручную при создании новой записи.
/// </summary>
public class BookTypeCreateUpdateDto
{
    /// <summary>
    /// Уникальный идентификатор типа книги.
    /// Устанавливается вручную при создании (обязателен и должен быть > 0).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название типа книги (например: "Роман", "Учебник", "Справочник").
    /// Обязательное поле, не может быть пусто.
    /// </summary>
    public required string Name { get; set; }
}
