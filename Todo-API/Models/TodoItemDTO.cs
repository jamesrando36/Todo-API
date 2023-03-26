namespace Todo_API.Models
{
    public class TodoItemDTO
    {
        public long Id { get; set; }
        public string? Task { get; set; }
        public bool IsComplete { get; set; }
    }
}
