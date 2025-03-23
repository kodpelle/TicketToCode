﻿using TicketToCode.Api.Endpoints;
public class GetBooking : IEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app) => app
        .MapGet("/bookings/{id}", Handle)
        .WithSummary("Get booking");

    public record Request(int Id);

    public record Response(
        int Id,
        int UserId,
        int EventId,
        int TicketCount,
        User User,
        Event Event,
        DateTime BookingDate
    );

    private static Response Handle([AsParameters] Request request, ApplicationDBContext db)
    {
        var booking = db.Bookings.Find(request.Id);
        if (booking == null)
        {
            throw new Exception("Booking not found");
        }

        var eventEntity = db.Events.Find(booking.EventId);
        if (eventEntity == null)
        {
            throw new Exception("Event not found");
        }

        return new Response(
            Id: booking.Id,
            UserId: booking.UserId,
            EventId: booking.EventId,
            TicketCount: booking.TicketCount,
            User: booking.User,
            Event: booking.Event,
            BookingDate: booking.BookingDate
        );
    }
}
