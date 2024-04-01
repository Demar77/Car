namespace Hedin.ChangeTires.Api.Services;

using System;
using System.Collections.Generic;
using Hedin.ChangeTires.Api.ExternalIntegrations;
using Hedin.ChangeTires.Api.Configurations;
using System.Net.Http;

public class SlotService
{
    private readonly ExternalSlotApiClient _externalSlotApiClient;

    public SlotService(ExternalServiceSettings settings)
    {
        _externalSlotApiClient = new ExternalSlotApiClient(new HttpClient(), settings);
    }

    public List<DateTime> GetAvailableSlots()
    {
        var slots = _externalSlotApiClient.GetAvailableSlots();

        return slots;
    }
}