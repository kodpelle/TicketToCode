using Microsoft.EntityFrameworkCore;
using TicketToCode.Api.Endpoints;
using TicketToCode.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7207").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

// Add services to the container.

//Gör att vi kan registrera ApplicationDBContext i DI-containern och konfigurea den att använda SQL med lokal databasfil "local.db"

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlite("Data Source=../TicketToCode.Core/local.db",
        b => b.MigrationsAssembly("TicketToCode.Core")));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Default mapping is /openapi/v1.json
builder.Services.AddOpenApi();



builder.Services.AddAuthentication()
    .AddCookie(IdentityConstants.ApplicationScheme);
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(PolicyConstants.Policies.Admin, policy => policy.RequireRole("Admin"));
});
builder.Services.AddIdentityCore<IdentityUser>().AddEntityFrameworkStores<ApplicationDBContext>().AddApiEndpoints();




builder.Services.AddScoped<SignInManager<IdentityUser>>();
var app = builder.Build();

app.UseCors("AllowBlazorClient");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    // Todo: consider scalar? https://youtu.be/Tx49o-5tkis?feature=shared
    app.UseSwaggerUI( options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
        options.DefaultModelsExpandDepth(-1);
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Endpoints 
app.MapEndpoints<Program>();
app.MapIdentityApi<IdentityUser>();

// Temporary endpoints
// Todo: Move to own files

// For more information on the logout endpoint and antiforgery, see:
// https://learn.microsoft.com/aspnet/core/blazor/security/webassembly/standalone-with-identity#antiforgery-support
app.MapPost("/logout", async (SignInManager<IdentityUser> signInManager, [FromBody] object empty) =>
{
    if (empty is not null)
    {
        await signInManager.SignOutAsync();

        return Results.Ok();
    }

    return Results.Unauthorized();
}).RequireAuthorization();


// provide an endpoint for user roles
app.MapGet("/roles", (ClaimsPrincipal user) =>
{
    if (user.Identity is null || !user.Identity.IsAuthenticated)
        return Results.Unauthorized();

    var identity = (ClaimsIdentity)user.Identity;
    var roles = identity.FindAll(identity.RoleClaimType)
        .Select(c =>
            new
            {
                c.Issuer,
                c.OriginalIssuer,
                c.Type,
                c.Value,
                c.ValueType
            });

    return TypedResults.Json(roles);
}).RequireAuthorization();


app.Run();

