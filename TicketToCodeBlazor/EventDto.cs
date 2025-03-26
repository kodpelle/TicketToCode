namespace TicketToCodeBlazor

{
    public class EventDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public EventType Type { get; set; } = EventType.Other;
        public DateTime Start{ get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.Now;
        public int MaxAttendees { get; set; }

    }
    public enum EventType
    {
        Concert = 0,
        Festival,
        Theatre,
        Other
    }
}
