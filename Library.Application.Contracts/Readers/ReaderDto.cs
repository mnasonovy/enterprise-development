namespace Library.Application.Contracts.Readers;

/// <summary>
/// DTO для отображения информации о читателе.
/// </summary>
public class ReaderDto
{
    /// <summary>
    /// Уникальный идентификатор читателя.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Полное имя читателя.
    /// </summary>
    public string FullName { get; set; } = default!;

    /// <summary>
    /// Адрес читателя.
    /// Необязательно.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Номер телефона читателя.
    /// Необязательно.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Дата регистрации читателя в системе библиотеки.
    /// </summary>
    public DateTime RegistrationDate { get; set; }
}
