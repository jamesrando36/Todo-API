namespace Todo_API.Entities
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string? Task { get; set; }
        public bool IsComplete { get; set; }
    }
}