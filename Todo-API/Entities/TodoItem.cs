namespace Todo_API.Entities
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string? Task { get; set; }
        public bool IsComplete { get; set; }
        public string? Description { get; set; }
        public DateTime? TaskTimestamp { get; set; }
    }
}