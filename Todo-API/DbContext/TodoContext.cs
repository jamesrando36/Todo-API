using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Todo_API.Entities;

namespace TodoApi.Models;

public class TodoContext : IdentityDbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<TodoItem> TodoItems { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoItem>()
            .HasData(
                new TodoItem() { Id = 1, Task = "Go to the gym", IsComplete = false },
                new TodoItem() { Id = 2, Task = "Learn something new", IsComplete = false },
                new TodoItem() { Id = 3, Task = "Clean up the house", IsComplete = false }
            );

        base.OnModelCreating(modelBuilder);
    }
}