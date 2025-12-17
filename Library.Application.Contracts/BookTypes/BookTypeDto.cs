namespace Library.Application.Contracts.BookTypes;

/// <summary>
/// DTO для чтения информации о типе книги.
/// Используется для возврата данных о типах книг в GET-запросах (например, /api/booktypes).
/// Содержит только ID и название типа.
/// </summary>
public class BookTypeDto
{
    /// <summary>
    /// Уникальный идентификатор типа книги.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название типа книги (например: "Роман", "Учебник", "Справочник").
    /// </summary>
    public string Name { get; set; } = default!;
}
