﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Todo_API.Entities;
using Todo_API.Exceptions;
using Todo_API.Models.TodoItemDtos;
using Todo_API.Services;
using TodoApi.Models;

namespace Todo_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IRepository _repository;
        private readonly IMapper _mapper;

        public TodoItemsController(IRepository repository,
            IMapper mapper)
        {
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
            var todoItems = await _repository.GetTodoItemsAsync();

            if (todoItems == null)
            {
                throw new NotFoundException("There are no todo items currently, please create an todo item and try again");
            }

            return Ok(_mapper.Map<IEnumerable<TodoItemDto>>(todoItems));
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
        public async Task<ActionResult<TodoItemDto>> GetTodoItem(long id)
        {
            if (!await _repository.TodoItemExistsAsync(id))
            {
                throw new NotFoundException("This todo item does not exist, please enter correct id, and try again");
            }

            var todoItem = await _repository.GetTodoItemAsync(id);

            return Ok(_mapper.Map<TodoItemDto>(todoItem));
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