using AutoMapper;
using Todo_API.Models.TodoItemDtos;

namespace Todo_API.Profiles
{
    public class TodoProfile : Profile
    {
        public TodoProfile()
        {
            CreateMap<Entities.TodoItem, TodoItemDto>();
            CreateMap<TodoItemCreateDto, Entities.TodoItem>();
            CreateMap<TodoItemUpdateDto, Entities.TodoItem>();
            CreateMap<Entities.TodoItem, TodoItemUpdateDto>();
        }
    }
}