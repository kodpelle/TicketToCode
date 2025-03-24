namespace TicketToCode.Api.Endpoints;

public class GetUserBookings: IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
        .MapGet("/bookings/{IdentityUserId}/bookings", Handle)
        .WithSummary("Get booking");
    public record Request(int UserId);
    public record Response(
        int Id,
        int UserId,
        int EventId,
        int TicketCount,
        DateTime BookingDate,
        Event Event
    );
    private static IEnumerable<Response> Handle([AsParameters] Request request, ApplicationDBContext db)
    {
        var response = new List<Response>();
        var bookings = db.Bookings.Where(b=>b.IdentityUserId==request.UserId).ToList();
        if (bookings == null|| !bookings.Any())
        {
            throw new Exception("Booking not found for this user");
        }

        foreach(var booking in bookings)
        {
            var eventEntity = db.Events.Find(booking.EventId);
            if (eventEntity == null)
            {
                throw new Exception("Event not found");
            }
            response.Add(new Response(
                Id: booking.Id,
                UserId: booking.IdentityUserId,
                EventId: booking.EventId,
                TicketCount: booking.TicketCount,
                Event: booking.Event,
                BookingDate: booking.BookingDate
            ));
        }
        return response;

    }
}
