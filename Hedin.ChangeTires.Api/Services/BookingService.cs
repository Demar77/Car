using Hedin.ChangeTires.Api.Configurations;
using Hedin.ChangeTires.Api.ExternalIntegrations;
using Hedin.ChangeTires.Api.Services.Interfaces;

namespace Hedin.ChangeTires.Api.Services;

public class BookingService : IBookingService
{
    private readonly IExternalSlotApiClient _externalSlotApiClient;

    public BookingService(ExternalServiceSettings settings)
    {
        _externalSlotApiClient = new ExternalSlotApiClient(new System.Net.Http.HttpClient(), settings);
    }

    public IResult BookTireChange(DateTime slot)
    {
        var bookingConfirmed = _externalSlotApiClient.ConfirmBooking(slot);

        if (bookingConfirmed)
        {
            return Results.Ok("Booking confirmed for " + slot.ToString("yyyy-MM-dd"));
        }
        else
        {
            return Results.NotFound("Booking could not be confirmed");
        }
    }
}
