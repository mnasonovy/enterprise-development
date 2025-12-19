using System.ComponentModel.DataAnnotations;

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
    [Range(1, int.MaxValue,
        ErrorMessage = "ID должен быть положительным числом")]
    public int Id { get; set; }

    /// <summary>
    /// Название издательства (например: "Penguin Random House", "AST", "Eksmo").
    /// Обязательное поле, не может быть пусто, максимум 100 символов.
    /// </summary>
    [Required(ErrorMessage = "Название издательства обязательно")]
    [StringLength(100, MinimumLength = 1,
        ErrorMessage = "Название должно быть от 1 до 100 символов")]
    public required string Name { get; set; }
}
