using System.ComponentModel.DataAnnotations;

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
    [Range(1, int.MaxValue,
        ErrorMessage = "ID должен быть положительным числом")]
    public int Id { get; set; }

    /// <summary>
    /// Полное имя читателя (например: "Иван Петров").
    /// Обязательное поле, не может быть пусто, максимум 150 символов.
    /// </summary>
    [Required(ErrorMessage = "Полное имя читателя обязательно")]
    [StringLength(150, MinimumLength = 1,
        ErrorMessage = "Имя должно быть от 1 до 150 символов")]
    public required string FullName { get; set; }

    /// <summary>
    /// Адрес читателя (например: "ул. Пушкина, д. 10, кв. 5").
    /// Необязательное поле, максимум 200 символов.
    /// </summary>
    [StringLength(200,
        ErrorMessage = "Адрес не должен превышать 200 символов")]
    public string? Address { get; set; }

    /// <summary>
    /// Номер телефона читателя (например: "+7 (999) 123-45-67").
    /// Необязательное поле, максимум 20 символов.
    /// </summary>
    [StringLength(20,
        ErrorMessage = "Номер телефона не должен превышать 20 символов")]
    public string? Phone { get; set; }

    /// <summary>
    /// Дата регистрации читателя в системе библиотеки (UTC).
    /// Обязательное поле, устанавливается вручную при создании.
    /// </summary>
    [Range(typeof(DateTime), "1900-01-01", "2100-12-31",
    ErrorMessage = "Дата регистрации должна быть между 1900 и 2100 годами")]
    public DateTime RegistrationDate { get; set; }

}
