using Library.Application.Contracts.Issues;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// REST API контроллер для полного управления выданными книгами (Issue).
/// 
/// Функциональность:
/// - Получение всех выданных книг с информацией о читателях и сроках
/// - Получение информации о конкретной выдаче по ID
/// - Создание новой записи о выдаче книги читателю
/// - Обновление информации о выданной книге (включая дату возврата)
/// - Удаление записи о выданной книге
/// 
/// Реализует полный CRUD (Create, Read, Update, Delete) цикл с подробным логированием,
/// валидацией входных данных и правильными HTTP статус-кодами для каждого сценария.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class IssuesController(
    IIssueService issueService,
    ILogger<IssuesController> logger) : ControllerBase
{
    /// <summary>
    /// Получить все выданные книги из системы с полной информацией.
    /// 
    /// Возвращаемые данные:
    /// - ID выдачи, ID книги, ID читателя
    /// - Название книги и полное имя читателя
    /// - Дата выдачи (IssueDate)
    /// - Количество дней выдачи (DaysCount)
    /// - Дата возврата (ReturnDate, если книга возвращена)
    /// - Полная информация о книге (автор, издатель)
    /// - Полная информация о читателе (контакты)
    /// 
    /// Ответы:
    /// - 200 OK: Список всех выданных книг с полной информацией (может быть пуст)
    /// - 204 No Content: Если в системе нет выданных книг
    /// - 500 Internal Server Error: При ошибке БД или обработки
    /// 
    /// Использование:
    /// - Для получения полного реестра выданной литературы
    /// - Для аудита и отчетности по выданным книгам
    /// - Для отслеживания текущего состояния книг в читательских руках
    /// - Для контроля сроков возврата
    /// </summary>
    /// <returns>Список всех выданных книг с полной информацией о книге, читателе и датах</returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<IssueDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetListAsync));

        var result = await issueService.GetListAsync();

        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetListAsync), result.Count);

        return result.Count > 0 ? Ok(result) : NoContent();
    }

    /// <summary>
    /// Получить полную информацию о конкретной выданной книге по идентификатору.
    /// 
    /// Возвращаемые данные:
    /// - ID выдачи и связанные ID (BookId, ReaderId)
    /// - Названия книги и полное имя читателя
    /// - Дата выдачи (IssueDate)
    /// - Количество дней выдачи (DaysCount)
    /// - Дата возврата (ReturnDate, если книга возвращена)
    /// - Информация об авторе и издателе книги
    /// - Информация о читателе (фамилия, имя, контакты)
    /// 
    /// Валидация:
    /// - ID должен быть больше 0
    /// - Выдача с этим ID должна существовать в БД
    /// 
    /// Ответы:
    /// - 200 OK: Полная информация о выданной книге
    /// - 400 Bad Request: ID некорректен (≤ 0)
    /// - 404 Not Found: Выдача с таким ID не найдена
    /// - 500 Internal Server Error: При ошибке БД
    /// 
    /// Использование:
    /// - Для проверки статуса конкретной выданной книги
    /// - Для получения информации о читателе, взявшем книгу
    /// - Для отслеживания сроков возврата книги
    /// - Для расчета переплат за просрочку
    /// </summary>
    /// <param name="id">Уникальный идентификатор выдачи (должен быть > 0)</param>
    /// <returns>Полное DTO выданной книги с информацией о книге, читателе и датах</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(IssueDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(GetAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        var result = await issueService.GetAsync(id);

        logger.LogInformation("{Method} method executed successfully", nameof(GetAsync));

        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Создать новую запись о выданной книге читателю.
    /// 
    /// Входные параметры (обязательны):
    /// - BookId: ID существующей книги в системе (> 0)
    /// - ReaderId: ID существующего читателя в системе (> 0)
    /// - IssueDate: Дата выдачи книги (не может быть default/пусто)
    /// - DaysCount: Количество дней, на которые выдается книга (> 0)
    /// - ReturnDate: Опционально - дата возврата (может быть null)
    /// 
    /// Процесс создания:
    /// - Валидирует все входные параметры
    /// - Проверяет существование книги и читателя в БД
    /// - Создает новую запись в БД
    /// - Автоматически загружает связанные сущности (Book и Reader)
    /// - Возвращает созданную запись с полной информацией
    /// 
    /// Ответы:
    /// - 201 Created: Запись успешно создана с заголовком Location
    /// - 400 Bad Request: Входные данные некорректны или отсутствуют обязательные поля
    /// - 500 Internal Server Error: При ошибке БД или обработки
    /// 
    /// Пример запроса:
    /// POST /api/issues
    /// {
    ///   "bookId": 1,
    ///   "readerId": 1,
    ///   "issueDate": "2025-01-10T00:00:00Z",
    ///   "daysCount": 14,
    ///   "returnDate": null
    /// }
    /// 
    /// Использование:
    /// - Для регистрации выданных читателям книг
    /// - Для отслеживания движения книг в библиотеке
    /// - Для расчета сроков возврата
    /// - Для ведения истории выдачи
    /// </summary>
    /// <param name="input">DTO с данными новой выдачи (все обязательные поля должны быть заполнены)</param>
    /// <returns>DTO созданной выдачи с заполненным ID и полной информацией</returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(IssueDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] IssueCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called", nameof(CreateAsync));

        if (input is null)
            return BadRequest("Issue data is required");

        if (input.BookId <= 0)
            return BadRequest("Book ID must be greater than 0");

        if (input.ReaderId <= 0)
            return BadRequest("Reader ID must be greater than 0");

        if (input.IssueDate == default)
            return BadRequest("Issue date is required");

        if (input.DaysCount <= 0)
            return BadRequest("Days count must be greater than 0");

        try
        {
            var result = await issueService.CreateAsync(input);

            logger.LogInformation("{Method} method executed successfully with id = {Id}",
                nameof(CreateAsync), result.Id);

            return Created($"/api/issues/{result.Id}", result);
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error creating Issue: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Обновить существующую запись о выданной книге.
    /// 
    /// Входные параметры (обязательны):
    /// - id (в URL): ID существующей выдачи (> 0)
    /// - BookId: ID книги (> 0)
    /// - ReaderId: ID читателя (> 0)
    /// - IssueDate: Дата выдачи (не может быть default)
    /// - DaysCount: Количество дней (> 0)
    /// - ReturnDate: Опционально - дата возврата (может быть null или содержать дату)
    /// 
    /// Сценарии использования:
    /// - Обновление информации о выданной книге
    /// - Установка даты возврата (ReturnDate) при возврате книги
    /// - Изменение срока выдачи (DaysCount) в случае продления
    /// - Переассоциирование читателя или книги (если требуется)
    /// 
    /// Процесс обновления:
    /// - Валидирует ID и входные параметры
    /// - Проверяет наличие записи в БД
    /// - Обновляет все поля (включая ReturnDate)
    /// - Возвращает обновленную запись с актуальной информацией
    /// 
    /// Ответы:
    /// - 200 OK: Запись успешно обновлена
    /// - 400 Bad Request: Входные данные некорректны или ID некорректен
    /// - 404 Not Found: Выдача с таким ID не найдена
    /// - 500 Internal Server Error: При ошибке БД или обработки
    /// 
    /// Пример запроса (возврат книги):
    /// PUT /api/issues/1
    /// {
    ///   "bookId": 1,
    ///   "readerId": 1,
    ///   "issueDate": "2025-01-10T00:00:00Z",
    ///   "daysCount": 14,
    ///   "returnDate": "2025-01-24T00:00:00Z"
    /// }
    /// 
    /// Использование:
    /// - Для отметки возврата книги (установка ReturnDate)
    /// - Для корректировки информации о выданной книге
    /// - Для продления срока выдачи (изменение DaysCount)
    /// - Для переназначения книги другому читателю
    /// </summary>
    /// <param name="id">Уникальный идентификатор выдачи для обновления (должен быть > 0)</param>
    /// <param name="input">DTO с новыми данными выдачи (все обязательные поля должны быть заполнены)</param>
    /// <returns>DTO обновленной выдачи с полной информацией</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(IssueDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] IssueCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(UpdateAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        if (input is null)
            return BadRequest("Issue data is required");

        if (input.BookId <= 0)
            return BadRequest("Book ID must be greater than 0");

        if (input.ReaderId <= 0)
            return BadRequest("Reader ID must be greater than 0");

        if (input.IssueDate == default)
            return BadRequest("Issue date is required");

        if (input.DaysCount <= 0)
            return BadRequest("Days count must be greater than 0");

        try
        {
            var result = await issueService.UpdateAsync(id, input);

            logger.LogInformation("{Method} method executed successfully", nameof(UpdateAsync));

            return result != null ? Ok(result) : NotFound();
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error updating Issue: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Удалить запись о выданной книге из системы по идентификатору.
    /// 
    /// Важно:
    /// - При удалении удаляется ТОЛЬКО запись выдачи (Issue)
    /// - Сама книга (Book) и читатель (Reader) остаются в системе
    /// - Удаление безвозвратно и не может быть отменено
    /// - История выдачи книг будет потеряна при удалении записи
    /// 
    /// Валидация:
    /// - ID должен быть больше 0
    /// - При успешном удалении возвращается пустой ответ (204 No Content)
    /// 
    /// Ответы:
    /// - 204 No Content: Запись успешно удалена
    /// - 400 Bad Request: ID некорректен (≤ 0)
    /// - 500 Internal Server Error: При ошибке БД или обработки
    /// 
    /// Сценарии использования:
    /// - Удаление ошибочно созданных записей о выдаче
    /// - Удаление устаревших записей при архивировании
    /// - Очистка БД от дублирующихся записей
    /// 
    /// Использование:
    /// DELETE /api/issues/1
    /// 
    /// Осторожно:
    /// - Операция необратима
    /// - Убедитесь, что удаляете правильную запись
    /// - История выдачи будет потеряна при удалении
    /// - Рекомендуется сделать резервную копию перед удалением
    /// </summary>
    /// <param name="id">Уникальный идентификатор выдачи для удаления (должен быть > 0)</param>
    /// <returns>204 No Content при успешном удалении, 400 или 500 при ошибке</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(DeleteAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        try
        {
            await issueService.DeleteAsync(id);

            logger.LogInformation("{Method} method executed successfully", nameof(DeleteAsync));

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error deleting Issue: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }
}
