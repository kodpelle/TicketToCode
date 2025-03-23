namespace TicketToCode.Api.Endpoints;

public class CreateBooking : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
   .MapPost("/bookings/create", Handle)
   .WithSummary("Create booking");

    public record Request(
     int UserId,
     int EventId,
     int TicketCount,
     DateTime BookingDate
     );
    public record Response(int id);
    private static async Task<IResult> Handle(Request request, ApplicationDBContext db)
    {
        var eventEntity = await db.Events.FindAsync(request.EventId);
        if (eventEntity == null)
        {
            return Results.NotFound("Eventet finns inte.");
        }

        // Kontrollera om det finns tillräckligt med biljetter
        if (eventEntity.MaxAttendees < request.TicketCount)
        {
            return Results.BadRequest("Inte tillräckligt med biljetter kvar.");
        }
        var b = new Booking();
        // Map request to an event-object

        b.UserId = request.UserId;
        b.EventId = request.EventId;    
        b.TicketCount = request.TicketCount;
        b.BookingDate = request.BookingDate;
        // Todo: does this set id on ev-object?
        db.Bookings.Add(b);

        await db.SaveChangesAsync();  // Viktigt! Sparar ändringarna i databasen.
        return TypedResults.Ok(new Response(b.Id));
    }
}

