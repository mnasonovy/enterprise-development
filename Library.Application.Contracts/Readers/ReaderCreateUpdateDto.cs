namespace Library.Application.Contracts.Readers;

/// <summary>
/// DTO для создания или обновления информации о читателе.
/// Используется при приеме данных от клиента для создания нового читателя или обновления существующего.
/// </summary>
public class ReaderCreateUpdateDto
{
    /// <summary>
    /// Полное имя читателя.
    /// Обязательное поле.
    /// </summary>
    public string FullName { get; set; } = default!;

    /// <summary>
    /// Адрес читателя.
    /// Необязательное поле.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Номер телефона читателя.
    /// Необязательное поле.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Дата регистрации читателя в системе библиотеки.
    /// </summary>
    public DateTime RegistrationDate { get; set; }
}
