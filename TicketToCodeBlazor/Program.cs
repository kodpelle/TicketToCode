using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http.Headers;

namespace TicketToCodeBlazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {


            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            var httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7206")
            };

            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));




           /* builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });*/
            builder.Services.AddScoped(sp => httpClient);
            builder.Services.AddScoped<EventService>();
            builder.Services.AddScoped<BookingService>();

            await builder.Build().RunAsync();
        }
    }
}
