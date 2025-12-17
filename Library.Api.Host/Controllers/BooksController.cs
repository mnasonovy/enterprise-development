using Library.Application.Contracts.Books;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Host.Controllers;

/// <summary>
/// REST API контроллер для управления книгами в каталоге библиотеки.
/// Реализует полный набор CRUD-операций: получение списка, получение по ID, 
/// создание новой книги, обновление и удаление.
/// Все методы содержат подробное логирование и валидацию входных данных.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class BooksController(
    IBookService bookService,
    ILogger<BooksController> logger) : ControllerBase
{
    /// <summary>
    /// Получает список всех книг из каталога.
    /// </summary>
    /// <returns>
    /// 200 OK с коллекцией DTO всех книг.
    /// 204 No Content если каталог пуст.
    /// 500 Internal Server Error в случае ошибки БД.
    /// </returns>
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IReadOnlyList<BookDto>))]
    [ProducesResponseType(204)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetListAsync()
    {
        logger.LogInformation("{Method} method is called", nameof(GetListAsync));
        var result = await bookService.GetListAsync();
        logger.LogInformation("{Method} method executed successfully with {Count} items",
            nameof(GetListAsync), result.Count);
        return result.Count > 0 ? Ok(result) : NoContent();
    }

    /// <summary>
    /// Получает информацию о книге по идентификатору.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги</param>
    /// <returns>
    /// 200 OK с DTO книги.
    /// 400 Bad Request если ID некорректен (≤ 0).
    /// 404 Not Found если книга не найдена.
    /// 500 Internal Server Error в случае ошибки БД.
    /// </returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(200, Type = typeof(BookDto))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> GetAsync(int id)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(GetAsync), id);
        if (id <= 0)
            return BadRequest("Id must be greater than 0");
        var result = await bookService.GetAsync(id);
        logger.LogInformation("{Method} method executed successfully", nameof(GetAsync));
        return result != null ? Ok(result) : NotFound();
    }

    /// <summary>
    /// Создает новую книгу в каталоге.
    /// Требует обязательные поля: ID (устанавливается вручную), Title, Year, BookTypeId, PublisherId.
    /// Авторы передаются списком AuthorIds и должны существовать в БД.
    /// </summary>
    /// <param name="input">DTO с данными новой книги</param>
    /// <returns>
    /// 201 Created с DTO созданной книги и заголовком Location.
    /// 400 Bad Request если входные данные некорректны или отсутствуют обязательные поля.
    /// 500 Internal Server Error в случае ошибки БД или обработки.
    /// </returns>
    [HttpPost]
    [ProducesResponseType(201, Type = typeof(BookDto))]
    [ProducesResponseType(400)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> CreateAsync([FromBody] BookCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called", nameof(CreateAsync));

        if (input == null)
            return BadRequest("Book data is required");

        if (input.Id <= 0)
            return BadRequest("Book ID must be set manually and be greater than 0");

        if (string.IsNullOrWhiteSpace(input.Title))
            return BadRequest("Book title is required");

        if (input.Year <= 0)
            return BadRequest("Book year must be greater than 0");

        if (input.BookTypeId <= 0)
            return BadRequest("BookType ID is required and must be greater than 0");

        if (input.PublisherId <= 0)
            return BadRequest("Publisher ID is required and must be greater than 0");

        try
        {
            var result = await bookService.CreateAsync(input);
            logger.LogInformation("{Method} method executed successfully with id = {Id}",
                nameof(CreateAsync), result.Id);
            return Created($"/api/books/{result.Id}", result);
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error creating Book: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Обновляет существующую книгу в каталоге.
    /// Требует обязательные поля: Title, Year, BookTypeId, PublisherId.
    /// Авторы передаются списком AuthorIds и должны существовать в БД.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для обновления</param>
    /// <param name="input">DTO с новыми данными книги</param>
    /// <returns>
    /// 200 OK с обновленным DTO книги.
    /// 204 No Content если книга не найдена.
    /// 400 Bad Request если входные данные или ID некорректны.
    /// 404 Not Found если книга не найдена в БД.
    /// 500 Internal Server Error в случае ошибки БД или обработки.
    /// </returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(200, Type = typeof(BookDto))]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    [ProducesResponseType(500)]
    public async Task<IActionResult> UpdateAsync(int id, [FromBody] BookCreateUpdateDto input)
    {
        logger.LogInformation("{Method} method is called with id = {Id}", nameof(UpdateAsync), id);

        if (id <= 0)
            return BadRequest("Id must be greater than 0");

        if (input == null)
            return BadRequest("Book data is required");

        if (string.IsNullOrWhiteSpace(input.Title))
            return BadRequest("Book title is required");

        if (input.Year <= 0)
            return BadRequest("Book year must be greater than 0");

        if (input.BookTypeId <= 0)
            return BadRequest("BookType ID is required and must be greater than 0");

        if (input.PublisherId <= 0)
            return BadRequest("Publisher ID is required and must be greater than 0");

        try
        {
            var result = await bookService.UpdateAsync(id, input);
            logger.LogInformation("{Method} method executed successfully", nameof(UpdateAsync));
            return result != null ? Ok(result) : NotFound();
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error updating Book: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Удаляет книгу из каталога по идентификатору.
    /// При удалении также удаляются все связанные выпуски (экземпляры) книги.
    /// </summary>
    /// <param name="id">Уникальный идентификатор книги для удаления</param>
    /// <returns>
    /// 204 No Content при успешном удалении.
    /// 400 Bad Request если ID некорректен (≤ 0).
    /// 500 Internal Server Error в случае ошибки БД или обработки.
    /// </returns>
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
            await bookService.DeleteAsync(id);
            logger.LogInformation("{Method} method executed successfully", nameof(DeleteAsync));
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            logger.LogError("Error deleting Book: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }
}
