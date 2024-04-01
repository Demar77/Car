namespace Hedin.ChangeTires.Api.Services;

using Hedin.ChangeTires.Api.Configurations;
using Hedin.ChangeTires.Api.ExternalIntegrations;
using Hedin.ChangeTires.Api.Models;
using System.Collections.Generic;
using System.Net.Http;

public class SlotService
{
    private readonly ExternalSlotApiClient _externalSlotApiClient;

    public SlotService(ExternalServiceSettings settings)
    {
        _externalSlotApiClient = new ExternalSlotApiClient(new HttpClient(), settings);
    }

    public List<Booking> GetAvailableSlots()
    {
        var slots = _externalSlotApiClient.GetBookedSlots();

        return slots;
    }
}