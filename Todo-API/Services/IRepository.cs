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
        /// Get filtered tasks
        /// </summary>
        /// <returns> a list of filtered tasks</returns>
        Task<IEnumerable<TodoItem>> GetFilteredTaskAsync(string? task, string? search);

        /// <summary>
        /// Creates a single todo item
        /// </summary>
        /// <param name="item"></param>
        /// <returns>the new created todo item</returns>
        Task CreateTodoItemAsync(TodoItem item);

        /// <summary>
        /// Delete a single todo item
        /// </summary>
        /// <param name="id"></param>
        /// <returns>deleted todo item</returns>
        Task DeleteTodoItemAsync(long id);

        /// <summary>
        /// Delete all single todo items
        /// </summary>
        /// <returns>deleted todo items</returns>
        Task DeleteAllTodoItemsAsync();

        /// <summary>
        /// Checks that a todo item exists
        /// </summary>
        /// <param name="itemId"></param>
        /// <returns> true if exists </returns>
        Task<bool> TodoItemExistsAsync(long itemId);

        /// <summary>
        /// Saves ef data changes to the db
        /// </summary>
        /// <returns>saved change to the db</returns>
        Task<bool> SaveChangesAsync();
    }
}