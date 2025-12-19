namespace Library.Application.Contracts.Readers;

/// <summary>
/// DTO для отображения информации о читателе.
/// Используется в GET запросах для отправки данных о читателе в API ответе.
/// </summary>
public class ReaderDto
{
    /// <summary>
    /// Уникальный идентификатор читателя.
    /// Это значение установлено клиентом при создании и хранится в БД.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Полное имя читателя (например: "Иван Петров").
    /// Обязательное поле, максимум 150 символов.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Адрес читателя (например: "ул. Пушкина, д. 10, кв. 5").
    /// Необязательное поле, максимум 200 символов.
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// Номер телефона читателя (например: "+7 (999) 123-45-67").
    /// Необязательное поле, максимум 20 символов.
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// Дата и время регистрации читателя в системе библиотеки (UTC).
    /// Установлено клиентом при создании и хранится в БД.
    /// </summary>
    public DateTime RegistrationDate { get; set; }
}
