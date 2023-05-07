using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Todo_API.Entities;
using Todo_API.Exceptions;
using Todo_API.Models.TodoItemDtos;
using Todo_API.Services;
using ILogger = Serilog.ILogger;

namespace Todo_API.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TodoItemsController(
            IRepository repository,
            IMapper mapper,
            ILogger logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Get all todo items
        /// </summary>
        /// <returns> all todo items</returns>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<TodoItemDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems()
        {
            try
            {
                var todoItems = await _repository.GetTodoItemsAsync();

                if (!todoItems.Any())
                {
                    _logger.Error("No todo items found in the repository");
                    return NotFound();
                }

                return Ok(_mapper.Map<IEnumerable<TodoItemDto>>(todoItems));
            }
            catch (NotFoundException ex) 
            {
                _logger.Error(ex, $"An error occurred while getting the todo items.");// clean errors into their own class
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Get all filtered tasks via filter
        /// </summary>
        /// <returns> all filtered task items</returns>
        [Route("filter")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems(string? task, string? search)
        {
            var todoItems = await _repository.GetFilteredTaskAsync(task, search);

            if (todoItems == null)
            {
                throw new NotFoundException("There are no todo items currently, please create an todo item and try again");
            }

            return Ok(_mapper.Map<IEnumerable<TodoItemDto>>(todoItems));
        }

        /// <summary>
        /// Get a single todo item
        /// </summary>
        /// <param name="id"></param>
        /// <returns> a single todo item </returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TodoItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTodoItem(long id)
        {
            try
            {
                if (!await _repository.TodoItemExistsAsync(id))
                {
                    throw new NotFoundException($"Todo item with ID {id} does not exist, please enter correct id, and try again");
                }

                var todoItem = await _repository.GetTodoItemAsync(id);

                if (todoItem == null)
                {
                    throw new Exception($"Failed to get the todo item with ID {id}.");
                }

                return Ok(_mapper.Map<TodoItemDto>(todoItem));
            }
            catch (NotFoundException ex)
            {
                _logger.Error(ex, $"Todo item with ID {id} does not exist.");
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred while getting the todo item.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"An error occurred while getting the todo item with ID {id}.");
            }
        }

        /// <summary>
        /// Update a todo item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="todoItem"></param>
        /// <returns>an updated todo item</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemUpdateDto todoItemUpdate)
        {
            if (!await _repository.TodoItemExistsAsync(id))
            {
                throw new NotFoundException("This todo item does not exist, please enter correct id, and try again");
            }

            var todoItemEntity = await _repository
                .GetTodoItemAsync(id);

            _mapper.Map(todoItemUpdate, todoItemEntity);

            await _repository.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        ///  Partial update for a todo item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patchDocument"></param>
        /// <returns>an updated todo item</returns>
        /// <exception cref="NotFoundException"></exception>
        [HttpPatch("{id}")]
        public async Task<ActionResult> PartiallyUpdatePointOfInterest(long id,
           JsonPatchDocument<TodoItemUpdateDto> patchDocument)
        {
            if (!await _repository.TodoItemExistsAsync(id))
            {
                throw new NotFoundException("This todo item does not exist, please enter correct id, and try again");
            }

            var todoItemEntity = await _repository
                .GetTodoItemAsync(id);

            var todoItemToPatch = _mapper.Map<TodoItemUpdateDto>(
                todoItemEntity);

            patchDocument.ApplyTo(todoItemToPatch);

            _mapper.Map(todoItemToPatch, todoItemEntity);

            await _repository.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Create a new todo item
        /// </summary>
        /// <param name="todoItemCreateDto"></param>
        /// <returns> Created todo item </returns>
        [HttpPost]
        public async Task<ActionResult<TodoItemDto>> CreateTodoItem(TodoItemCreateDto todoItemCreateDto)
        {
            var todoitem = _mapper.Map<TodoItem>(todoItemCreateDto);

            await _repository.CreateTodoItemAsync(todoitem);

            var createdTodoItem =
                _mapper.Map<TodoItem>(todoitem);

            return CreatedAtRoute(
                 new
                 {
                     id = todoitem.Id,
                 },
                 createdTodoItem);
        }

        /// <summary>
        /// Deletes a single to do item from the collection
        /// </summary>
        /// <param name="id"> id of todo item </param>
        /// <returns> deleted todo item </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            if (!await _repository.TodoItemExistsAsync(id))
            {
                throw new NotFoundException("This todo item does not exist, please enter correct id, and try again");
            }

            await _repository.DeleteTodoItemAsync(id);

            return NoContent();
        }
    }
}