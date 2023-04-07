﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Todo_API.Entities;
using Todo_API.Models.TodoItemDtos;
using Todo_API.Services;
using TodoApi.Models;

namespace Todo_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public TodoItemsController(TodoContext context,
            IRepository repository,
            IMapper mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Get all todo items
        /// </summary>
        /// <returns> all todo items</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDto>>> GetTodoItems()
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }

            var todoItems = await _repository.GetTodoItemsAsync();

            return Ok(_mapper.Map<IEnumerable<TodoItemDto>>(todoItems));
        }

        /// <summary>
        /// Get a single todo item
        /// </summary>
        /// <param name="id"></param>
        /// <returns> a single todo item </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDto>> GetTodoItem(long id)
        {
            var todoItem = await _repository.GetTodoItemAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<TodoItemDto>>(todoItem));
        }

        /// <summary>
        /// Update a todo item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="todoItem"></param>
        /// <returns>an updated todo iteam</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItemUpdateDto todoItemUpdate)
        {
            if (!await _repository.TodoItemExistsAsync(id))
            {
                return NotFound();
            }

            var todoItemEntity = await _repository
                .GetTodoItemAsync(id);


            if (todoItemEntity == null)
            {
                return NotFound();
            }

            _mapper.Map(todoItemUpdate, todoItemEntity);

            await _context.SaveChangesAsync();

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

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}