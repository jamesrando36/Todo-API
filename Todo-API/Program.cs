using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Todo_API.Services;
using TodoApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TodoContext>(
    dbContextOptions => dbContextOptions.UseSqlServer(builder.Configuration
    .GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseSwagger();

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    options.RoutePrefix = string.Empty;
});

app.MapControllers();

app.Run();