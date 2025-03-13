using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using TicketToCode.Core.Data;
using TicketToCode.Core.Models;

namespace TicketToCode.Api.Endpoints
{
    public class BookingEndpoints : IEndpoint
    {
        public static void MapEndpoint(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/bookings");

            // Skapa en bokning
            group.MapPost("/", async (BookingDto bookingDto, ApplicationDbContext db) =>
            {
                var booking = new Booking
                {
                    UserId = bookingDto.UserId,
                    EventId = bookingDto.EventId,
                    BookingDate = bookingDto.BookingDate
                };

                db.Bookings.Add(booking);
                await db.SaveChangesAsync();

                return Results.Ok(new { message = "Bokning skapad!", bookingId = booking.Id });
            });

            // Hämta alla bokningar
            group.MapGet("/", async (ApplicationDbContext db) =>
                Results.Ok(await db.Bookings.ToListAsync())
            );
        }
    }

    public class BookingDto
    {
        public int UserId { get; set; }
        public int EventId { get; set; }
        public DateTime BookingDate { get; set; }
    }
}
