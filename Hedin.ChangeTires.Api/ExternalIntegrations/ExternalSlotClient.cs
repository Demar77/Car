using Hedin.ChangeTires.Api.Configurations;

namespace Hedin.ChangeTires.Api.ExternalIntegrations
{
    public class ExternalSlotApiClient : IExternalSlotApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ExternalServiceSettings _settings;

        public ExternalSlotApiClient(HttpClient httpClient, ExternalServiceSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public List<Booking> GetBookedSlots()
        {
            var response = _httpClient.GetAsync(_settings.Url + "/slotsext").Result;
            response.EnsureSuccessStatusCode();

            var slots = response.Content.ReadFromJsonAsync<List<Booking>>().Result;
            return slots ?? new List<Booking>();
        }

        public bool ConfirmBooking(DateTime slot)
        {
            var response = _httpClient.PostAsJsonAsync(_settings.Url + "/slotsext", slot).Result;
            return response.IsSuccessStatusCode;
        }
    }
}