using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Headers;
using TicketToCodeBlazor.Identity;

namespace TicketToCodeBlazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {


            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // register the cookie handler
            builder.Services.AddTransient<CookieHandler>();

            // set up authorization
            builder.Services.AddAuthorizationCore();

            // register the custom state provider
            builder.Services.AddScoped<AuthenticationStateProvider, CookieAuthenticationStateProvider>();

            // register the account management interface
            builder.Services.AddScoped(
                sp => (IAccountManagement)sp.GetRequiredService<AuthenticationStateProvider>());

            // set base address for default host
            // Where the blazor wasm is hosted (right now, our devServer)
            builder.Services.AddScoped(sp =>
                new HttpClient { BaseAddress = new Uri(builder.Configuration["FrontendUrl"] ?? "https://localhost:7207/") });

            // configure client for auth interactions
            builder.Services.AddHttpClient(
                "Auth",
                opt => opt.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:7206"))
                .AddHttpMessageHandler<CookieHandler>();





            /* builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });*/
            builder.Services.AddScoped<EventService>();
            builder.Services.AddScoped<BookingService>();

            await builder.Build().RunAsync();
        }
    }
}
