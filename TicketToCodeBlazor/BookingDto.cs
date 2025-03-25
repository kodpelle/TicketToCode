namespace TicketToCodeBlazor
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public int TicketCount { get; set; }
        public DateTime BookingDate { get; set; }
    }
}

