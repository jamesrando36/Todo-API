using Microsoft.AspNetCore.Mvc;
using Todo_API.Entities;

namespace Todo_API.Services
{
    public interface IRepository
    {
        /// <summary>
        /// Get all todo items
        /// </summary>
        /// <returns> a list of todo items</returns>
        Task<IEnumerable<TodoItem>> GetTodoItemsAsync();

        /// <summary>
        /// Get a single todo item
        /// </summary>
        /// <param name="id">todo item id</param>
        /// <returns> a single todo item </returns>
        Task<TodoItem?> GetTodoItemAsync(long id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task CreateTodoItemAsync(TodoItem item);

        /// <summary>
        /// Checks that a todo item exists
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns> true if exists </returns>
        Task<bool> TodoItemExistsAsync(long itemId);



    }
}