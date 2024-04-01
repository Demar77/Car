namespace Hedin.ChangeTires.Api.ExternalIntegrations
{
    public interface IExternalSlotApiClient
    {
        List<Booking> GetBookedSlots();

        bool ConfirmBooking(DateTime slot);
    }
}