
namespace TicketToCode.Api.Endpoints;

public class Delete:IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
        .MapDelete("/events/{id}", Handle)
        .WithSummary("Delete an event");

    private static async Task<IResult> Handle(int id, ApplicationDBContext db)
    {
        var ev = await db.Events.FindAsync(id);
        if (ev == null)
        {
            return TypedResults.NotFound("Event not found");
        }
        db.Events.Remove(ev);
        await db.SaveChangesAsync();
        return TypedResults.NotFound();
    }


   
}

