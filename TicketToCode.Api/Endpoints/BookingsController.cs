using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[Authorize] // Säkerställer att endast inloggade användare kan komma åt API:et
[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public BookingsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    // Hjälpmetod för att hämta inloggad användares ID
    private async Task<string> GetUserIdAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        return user?.Id;
    }

    // Hämta alla bokningar för inloggad användare
    [HttpGet("bookings")]
    public async Task<IActionResult> GetUserBookings()
    {
        var userId = await GetUserIdAsync();
        if (userId == null) return Unauthorized(); // Om användaren inte är inloggad

        var bookings = await _context.Bookings
            .Where(b => b.UserId == userId)
            .Include(b => b.Event) // Inkludera event-information
            .ToListAsync();

        return bookings.Any() ? Ok(bookings) : NoContent();
    }

    // Avboka en bokning
    [HttpDelete("cancel/{id}")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        var userId = await GetUserIdAsync();
        if (userId == null) return Unauthorized(); // Om användaren inte är inloggad

        var booking = await _context.Bookings
            .Where(b => b.Id == id && b.UserId == userId)
            .FirstOrDefaultAsync();

        if (booking == null)
        {
            return NotFound(new { message = "Bokningen hittades inte eller tillhör inte dig." });
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return NoContent();
        }
        catch
        {
            await transaction.RollbackAsync();
            return StatusCode(500, new { message = "Ett fel uppstod vid avbokningen." });
        }
    }
}
