namespace Library.Application.Contracts.Readers;

/// <summary>
/// DTO для создания и обновления информации о читателе.
/// Используется при приеме данных от клиента для создания нового читателя или обновления существующего.
/// Содержит ID, устанавливаемый вручную при создании новой записи.
/// </summary>
public class ReaderCreateUpdateDto
{
    /// <summary>
    /// Уникальный идентификатор читателя.
    /// Устанавливается вручную при создании (обязателен и должен быть > 0).
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Полное имя читателя.
    /// Обязательное поле, не может быть пусто или содержать только пробелы.
    /// </summary>
    public required string FullName { get; set; }

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
    /// Обязательное поле, не может быть default (DateTime.MinValue).
    /// </summary>
    public DateTime RegistrationDate { get; set; }
}
