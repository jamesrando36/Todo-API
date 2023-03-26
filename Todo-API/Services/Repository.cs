using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Todo_API.Entities;
using TodoApi.Models;

namespace Todo_API.Services
{
    public class Repository : IRepository
    {
        private readonly TodoContext _context;

        public Repository(TodoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<TodoItem>> GetTodoItemsAsync()
        {
            return await _context.TodoItems.ToListAsync();
        }
    }
}
