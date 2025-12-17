namespace Library.Application.Contracts.Publishers;

/// <summary>
/// DTO для создания и обновления издателя.
/// Используется в POST и PUT запросах для передачи данных об издательстве (например: "Penguin", "AST", "Eksmo").
/// Отправляется клиентом в теле HTTP-запроса и десериализуется в этот объект.
/// </summary>
public class PublisherCreateUpdateDto
{
    /// <summary>
    /// Название издательства (например: "Penguin Random House", "Bloomsbury", "AST").
    /// Обязательное поле, не может быть пусто или содержать только пробелы.
    /// Максимальная длина: 256 символов.
    /// Используется для идентификации и отображения издателя в интерфейсе пользователя.
    /// </summary>
    public required string Name { get; set; }
}
