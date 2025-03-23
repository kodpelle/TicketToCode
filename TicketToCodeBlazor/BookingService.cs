//using System.Net.Http;
//using System.Net.Http.Json;
//using System.Threading.Tasks;
//using System.Collections.Generic;

//namespace TicketToCodeBlazor
//{
//    public class BookingService
//    {
//        private readonly HttpClient _httpClient;
//        public BookingService(HttpClient httpClient)
//        {
//            _httpClient = httpClient;
//        }

//        public async Task<Booking> GetBookingById()
//        {
//            return await _httpClient.GetFromJsonAsync<List<EventDto>>("https://localhost:7206/events") ?? new List<EventDto>();
//        }
//        public async Task<EventDto?> GetEventById(int id)
//        {
//            return await _httpClient.GetFromJsonAsync<EventDto>($"https://localhost:7206/events/{id}");
//        }

//        public async Task<int?> CreateEvent(EventDto newEvent)
//        {
//            var response = await _httpClient.PostAsJsonAsync("https://localhost:7206/events", newEvent);
//            if (response.IsSuccessStatusCode)
//            {
//                var result = await response.Content.ReadFromJsonAsync<CreateEventResponse>();
//                return result?.Id;
//            }
//            return null;

//        }
//    }
//}
