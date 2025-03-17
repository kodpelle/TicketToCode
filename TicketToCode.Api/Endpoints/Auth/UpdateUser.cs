namespace TicketToCode.Api.Endpoints;

public class UpdateUser : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
        .MapPut("/auth/{id}", Handle)
        .WithSummary("Update a user");
    private static async Task<IResult> Handle(int id, UpdateUserRequest request, ApplicationDBContext db)
    {
        var user = await db.Users.FindAsync(id);
        if (user == null)
        {
            return TypedResults.NotFound("User not found");
        }
        user.Username = request.Username;
        user.PasswordHash = request.Password;
        await db.SaveChangesAsync();
        return TypedResults.Ok();
    }
    public record UpdateUserRequest(
        string Username,
        string Password
    );
}
