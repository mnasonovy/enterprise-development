namespace Library.Application.Contracts.BookTypes;

/// <summary>
/// DTO для чтения информации о типе книги.
/// Используется для возврата данных о типах книг в GET-запросах (например, /api/booktypes).
/// </summary>
public class BookTypeDto
{
    /// <summary>
    /// Уникальный идентификатор типа книги.
    /// Это значение установлено клиентом при создании и хранится в БД.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Название типа книги (например: "Роман", "Учебник", "Справочник").
    /// Обязательное поле, максимум 50 символов.
    /// </summary>
    public required string Name { get; set; }
}
