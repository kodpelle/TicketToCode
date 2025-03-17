
namespace TicketToCode.Api.Endpoints;

public class DeleteUser : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)=> app
        .MapDelete("/auth/{id}", Handle)
        .WithSummary("Delete a user");

   private static async Task<IResult> Handle(int id, ApplicationDBContext db)
    {
        var user = await db.Users.FindAsync(id);
        if (user == null)
        {
            return TypedResults.NotFound("User not found");
        }
        db.Users.Remove(user);
        await db.SaveChangesAsync();
        return TypedResults.Ok();
    }

}
