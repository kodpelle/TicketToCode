namespace TicketToCode.Api.Endpoints;

public class GetAllBookings:IEndpoint
{
    // Mapping
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
        .MapGet("/bookings", Handle)
        .WithSummary("Get all bookings");

    // Request and Response types
    public record Response(
        int Id,
        int UserId,
        int EventId,
        int TicketCount,
        DateTime BookingDate
    );

    //Logic
    private static List<Response> Handle(ApplicationDBContext db)
    {
        var response = new List<Response>();
        foreach (var item in db.Bookings)
        {
            response.Add(new Response(
                Id: item.Id,
                UserId: item.IdentityUserId,
                EventId: item.EventId,
                TicketCount: item.TicketCount,
                BookingDate: item.BookingDate

          ));
        }
        return response;
    }
}
