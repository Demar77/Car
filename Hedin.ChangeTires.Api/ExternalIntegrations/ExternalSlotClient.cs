using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using Hedin.ChangeTires.Api.Configurations;

namespace Hedin.ChangeTires.Api.ExternalIntegrations
{
    public class ExternalSlotApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ExternalServiceSettings _settings;

        public ExternalSlotApiClient(HttpClient httpClient, ExternalServiceSettings settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }
        public List<DateTime> GetAvailableSlots()
        {
            var response = _httpClient.GetAsync(_settings.Url + "/slots").Result;
            response.EnsureSuccessStatusCode();
            
            var slots = response.Content.ReadFromJsonAsync<List<DateTime>>().Result;
            return slots ?? new List<DateTime>();
        }

        public bool ConfirmBooking(DateTime slot)
        {
            var response = _httpClient.PostAsJsonAsync(_settings.Url + "/slots", slot).Result;
            return response.IsSuccessStatusCode;
        }
    }
}