namespace Hedin.ChangeTires.Api.Services.Interfaces;

public interface IBookingService
{
    IResult BookTireChange(DateTime slot);
}