namespace TicketToCode.Api.Endpoints;
public class DeleteBooking : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
    .MapDelete("/bookings/{id}", Handle)
    .WithSummary("Delete an booking");

    private static async Task<IResult> Handle(int id, ApplicationDBContext db)
    {
        var b = await db.Bookings.FindAsync(id);
        if (b == null)
        {
            return TypedResults.NotFound("Event not found");
        }
        db.Bookings.Remove(b);
        await db.SaveChangesAsync();
        return TypedResults.NotFound();
    }

}

