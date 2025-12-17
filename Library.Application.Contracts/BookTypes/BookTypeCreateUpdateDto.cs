namespace Library.Application.Contracts.BookTypes;

/// <summary>
/// DTO для создания и обновления типа книги.
/// Используется в POST и PUT запросах для передачи данных о типе книги (Роман, Учебник, Справочник и т.д.).
/// </summary>
public class BookTypeCreateUpdateDto
{
    /// <summary>
    /// Название типа книги (например: "Роман", "Учебник", "Справочник").
    /// Обязательное поле, не может быть пусто.
    /// </summary>
    public required string Name { get; set; }
}
