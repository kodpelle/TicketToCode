using Microsoft.EntityFrameworkCore;
using TicketToCode.Api.Endpoints;
using TicketToCode.Core.Data;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorClient", policy =>
    {
        policy.WithOrigins("https://localhost:7207").AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

// Add services to the container.

//G�r att vi kan registrera ApplicationDBContext i DI-containern och konfigurea den att anv�nda SQL med lokal databasfil "local.db"

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseSqlite("Data Source=../TicketToCode.Core/local.db",
        b => b.MigrationsAssembly("TicketToCode.Core")));
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// Default mapping is /openapi/v1.json
builder.Services.AddOpenApi();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
    options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
})
    .AddCookie(IdentityConstants.ApplicationScheme, options =>
    {
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.None; // Viktigt f�r WASM
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Kr�ver HTTPS
    })
    .AddBearerToken(IdentityConstants.BearerScheme); // L�gg till Bearer Token

builder.Services.AddAuthorization();
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


app.MapEndpoints<Program>();
app.MapIdentityApi<IdentityUser>();

app.Run();

