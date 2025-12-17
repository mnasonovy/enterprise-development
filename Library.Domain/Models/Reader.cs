namespace Library.Domain.Models;

/// <summary>
/// Представляет читателя библиотеки с личными данными и информацией о регистрации.
/// </summary>
public class Reader
{
    /// <summary>
    /// Получает или задает уникальный идентификатор читателя.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Получает или задает полное имя читателя.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Получает или задает адрес читателя.
    /// Необязательно.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Получает или задает номер телефона читателя.
    /// Необязательно.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Получает или задает дату регистрации читателя в системе библиотеки.
    /// </summary>
    public DateTime RegistrationDate { get; set; }

    /// <summary>
    /// Навигационное свойство для проблем, связанных с этим читателем.
    /// </summary>
    public List<Issue> Issues { get; set; } = [];
}
