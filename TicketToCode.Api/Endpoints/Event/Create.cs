namespace TicketToCode.Api.Endpoints;
public class CreateEvent : IEndpoint
{
    // Mapping
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
        .MapPost("/events", Handle)
        .WithSummary("Create event");

    // Request and Response types
    // Why do we need these? check this video if you are not sure
    // https://youtu.be/xtpPspNdX58?si=GJBUxMzeR2ZJ5Fg_
    public record Request(
        string Name,
        string Description,
        EventType Type,
        DateTime Start,
        DateTime End,
        int MaxAttendees
        );
    public record Response(int id);

    //Logic
    // Ok<Response> är statisk och stödjer inte async(gör att man kan spara i databasen) så vi måste ändra till nedan
    private static async Task<IResult> Handle(Request request, ApplicationDBContext db)
    {
        // Todo, use a better constructor that enforces setting all necessary properties
        var ev = new Event();

        // Map request to an event-object
        ev.Name = request.Name;
        ev.Description = request.Description;
        ev.Type = request.Type;
        ev.StartTime = request.Start;
        ev.EndTime = request.End;
        ev.MaxAttendees = request.MaxAttendees;


        // Todo: does this set id on ev-object?
        db.Events.Add(ev); 

            await db.SaveChangesAsync();  // Viktigt! Sparar ändringarna i databasen.


        return TypedResults.Ok(new Response(ev.Id));
    }
}

