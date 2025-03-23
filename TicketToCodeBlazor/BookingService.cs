using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TicketToCodeBlazor
{
    public class BookingService
    {
        private readonly HttpClient _httpClient;
        public BookingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<BookingDto>> GetAllBookings()
        {
            return await _httpClient.GetFromJsonAsync<List<BookingDto>>("https://localhost:7206/bookings") ?? new List<BookingDto>();
        }
        public async Task<BookingDto?> GetBookingById(int id)
        {
            return await _httpClient.GetFromJsonAsync<BookingDto>($"https://localhost:7206/bookings/{id}");
        }

        public async Task<int?> CreateBooking(BookingDto newBooking)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7206/bookings/create", newBooking);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<CreateBookingResponse>();
                return result?.Id;
            }
            return null;

        }
        public async Task DeleteBooking(int id)
        {
            await _httpClient.DeleteAsync($"https://localhost:7206/bookings/{id}/delete");
        }

        public async Task GetUserBookings(int userId)
        {
            await _httpClient.GetFromJsonAsync<List<BookingDto>>($"https://localhost:7206/bookings/{userId}/bookings");
        }

    }
    public class CreateBookingResponse
    {
        public int Id { get; set; }
    }
 
}
