using Microsoft.EntityFrameworkCore;
using System;
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
            if (_context == null) throw new ArgumentNullException();

            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem?> GetTodoItemAsync(long id)
        {
            return await _context.TodoItems.Where(ti => ti.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task CreateTodoItemAsync(TodoItem todoItem)
        {
            _context.TodoItems.Add(todoItem);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> TodoItemExistsAsync(int itemId)
        {
            return await _context.TodoItems.AnyAsync(ti => ti.Id == itemId);
        }
    }
}