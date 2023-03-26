using Todo_API.Entities;

namespace Todo_API.Services
{
    public interface IRepository
    {
        Task<IEnumerable<TodoItem>> GetTodoItemsAsync();
    }
}
