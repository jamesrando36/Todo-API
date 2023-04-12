using System.ComponentModel.DataAnnotations;

namespace Todo_API.Models.TodoItemDtos
{
    public class TodoItemCreateDto
    {
        [Required(ErrorMessage = "a task is required to be able to create todo item")]
        [MaxLength(100)]
        public string? Task { get; set; }

        public bool IsComplete { get; set; }

        [MaxLength(100)]
        public string? Description { get; set; }

        public DateTime? TaskTimestamp { get; set; }
    }
}