namespace TicketToCodeBlazor
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int TicketCount { get; set; }
        public string BookingDate { get; set; }
        public User User { get; set; }
        public Event Event { get; set; }
    }
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

    public class Event
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
