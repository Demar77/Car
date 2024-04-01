using Hedin.ChangeTires.Api.Configurations;

namespace Hedin.ChangeTires.Api.Services;

using System;
using Hedin.ChangeTires.Api.ExternalIntegrations;

public class BookingService
{
    private readonly ExternalSlotApiClient _externalSlotApiClient;

    public BookingService(ExternalServiceSettings settings)
    {
        _externalSlotApiClient = new ExternalSlotApiClient(new System.Net.Http.HttpClient(), settings);
    }

    public string BookTireChange(DateTime slot)
    {
        var bookingConfirmed = _externalSlotApiClient.ConfirmBooking(slot);
        
        if (bookingConfirmed)
        {
            return "Booking confirmed for " + slot.ToString("yyyy-MM-dd");
        }
        else
        {
            return "Booking could not be confirmed";
        }
    }
}