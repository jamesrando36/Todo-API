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

        public async Task<IEnumerable<TodoItem>> GetFilteredTaskAsync(string? task, string? search)
        {
            if (_context == null) throw new ArgumentNullException();

            if (string.IsNullOrEmpty(task) && string.IsNullOrEmpty(search))
            {
                return await GetTodoItemsAsync();
            }

            var collection = _context.TodoItems as IQueryable<TodoItem>;

            if (!string.IsNullOrWhiteSpace(task))
            {
                task = task.Trim();
                collection = collection.Where(x => x.Task == task);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                collection = collection.Where(ti => ti.Task.Contains(search)
                    || (ti.Description != null && ti.Description.Contains(search)));
            }

            return await collection.OrderBy(ti => ti.Task).ToListAsync();
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

        public async Task DeleteTodoItemAsync(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            _context.TodoItems.Remove(todoItem);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> TodoItemExistsAsync(long itemId)
        {
            return await _context.TodoItems.AnyAsync(ti => ti.Id == itemId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}