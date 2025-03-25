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
            var response = await _httpClient.GetFromJsonAsync<List<BookingDto>>("https://localhost:7206/bookings");
            return response ?? new List<BookingDto>();
        }


        public async Task<BookingDto?> GetBookingById(int id)
        {
            return await _httpClient.GetFromJsonAsync<BookingDto>($"https://localhost:7206/bookings/{id}");
        }




        public async Task<int?> CreateBookingAsync(BookingDto booking)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7206/bookings/create", booking);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<CreateBookingResponse>();
                    return result?.Id;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Failed to create booking: {response.StatusCode} - {error}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception while creating booking: {ex.Message}");
                return null;
            }
        }




        public async Task<bool> DeleteBooking(int id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7206/bookings/{id}");
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to delete booking {id}: {response.StatusCode} - {error}");
                return false;
            }
            return true;
        }

    }
    public class CreateBookingResponse
    {
        public int Id { get; set; }
    }
 
}
