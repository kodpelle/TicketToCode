using Microsoft.EntityFrameworkCore;
using TicketToCode.Api.Endpoints;
using TicketToCode.Api.Services;
using TicketToCode.Core.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//G�r att vi kan registrera ApplicationDBContext i DI-containern och konfigurea den att anv�nda SQL med lokal databasfil "local.db"

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlite("Data Source=../TicketToCode.Core/local.db",
        b => b.MigrationsAssembly("TicketToCode.Core")));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Default mapping is /openapi/v1.json
builder.Services.AddOpenApi();
 
builder.Services.AddSingleton<IDatabase, Database>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Add cookie authentication
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.Cookie.Name = "auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

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

// Map all endpoints
app.MapEndpoints<Program>();
app.MapEndpoints<BookingEndpoints>();
group.MapGet("/events/{eventId}/available-dates", async (int eventId, ApplicationDbContext db) =>
{
    var eventDates = await db.EventDates
        .Where(e => e.EventId == eventId)
        .Select(e => e.AvailableDate)
        .ToListAsync();

    return eventDates.Any() ? Results.Ok(eventDates) : Results.NotFound("Inga datum tillgängliga.");
});

app.Run();



