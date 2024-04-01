namespace Hedin.ChangeTires.Api.Services.Interfaces;

public interface IPricingService
{
    Price CalculatePrice(Car car);
}