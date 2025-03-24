/*using TicketToCode.Core.Services;

namespace TicketToCode.Api.Endpoints.Auth;

public class Register : IEndpoint
{
    // Mapping
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
        .MapPost("/auth/register", Handle)
        .WithSummary("Register a new user")
        .AllowAnonymous();

    // Models
    public record Request(string Username, string Password);
    public record Response(string Username, string Role);

    // Logic
    private static Results<Ok<Response>, BadRequest<string>> Handle(
        Request request,
        IAuthService authService)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return TypedResults.BadRequest("Username and password are required");
        }
        
        var result = authService.Register(request.Username, request.Password);
        if (result == null)
            {
            return TypedResults.BadRequest("Username already exists");
            }
            
            var response = new Response(result.Username, result.Role);
            return TypedResults.Ok(response);
    }
}*/