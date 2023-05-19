namespace Todo_API.Models.TodoItemDtos
{
    public class TodoItemDto
    {
        public long Id { get; set; }

        public string? Task { get; set; }

        public bool IsComplete { get; set; }

        public string? Description { get; set; }

        public DateTime? TaskTimestamp { get; set; }

        public string? FormattedTaskTimestamp { get; set; }
    }
}