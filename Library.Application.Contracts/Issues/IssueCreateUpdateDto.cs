namespace Library.Application.Contracts.Issues;

/// <summary>
/// DTO для создания и обновления выданной книги.
/// Используется в POST (создание) и PUT (обновление/возврат) запросах.
/// </summary>
public class IssueCreateUpdateDto
{
    /// <summary>Уникальный идентификатор выдачи (обязателен для MongoDB)</summary>
    public int Id { get; set; }

    /// <summary>ID книги, которая выдается (обязателен)</summary>
    public int BookId { get; set; }

    /// <summary>ID читателя, который берет книгу (обязателен)</summary>
    public int ReaderId { get; set; }

    /// <summary>Дата выдачи книги (обязательна)</summary>
    public DateTime IssueDate { get; set; }

    /// <summary>Количество дней выдачи (обязательно, > 0)</summary>
    public int DaysCount { get; set; }

    /// <summary>Дата возврата книги (опционально, null если не возвращена)</summary>
    public DateTime? ReturnDate { get; set; }
}