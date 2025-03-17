namespace TicketToCode.Api.Endpoints;

public class Update : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
        .MapPut("/events/{id}", Handle)
        .WithSummary("Update an event");
    private static async Task<IResult> Handle(int id,UpdateEvent request, ApplicationDBContext db)
    {
        var ev = await db.Events.FindAsync(id);
        if (ev == null)
        {
            return TypedResults.NotFound("Event not found");
        }
        ev.Name = request.Name;
        ev.Description = request.Description;
        ev.Type = request.Type;
        ev.StartTime = request.Start;
        ev.EndTime = request.End;
        ev.MaxAttendees = request.MaxAttendees;
        await db.SaveChangesAsync();
        return TypedResults.Ok();
    }
    public record UpdateEvent(
        string Name,
        string Description,
        EventType Type,
        DateTime Start,
        DateTime End,
        int MaxAttendees
    );
}
