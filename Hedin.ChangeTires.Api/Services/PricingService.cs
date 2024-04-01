using Hedin.ChangeTires.Api.Models.Enums;
using Hedin.ChangeTires.Api.Services.Interfaces;
using Hedin.ChangeTires.Api.Strategies.Interfaces;

namespace Hedin.ChangeTires.Api.Services;

public class PricingService : IPricingService
{
    private const int WheelAlignmentPrice = 50;
    private readonly ITirePricing _tirePricing;
    private readonly ICarTypePricing _carTypePricing;
    private readonly ILogger<PricingService> _logger;

    public PricingService(ITirePricing tirePricing, ICarTypePricing carTypePricing, ILogger<PricingService> logger)
    {
        _tirePricing = tirePricing;
        _carTypePricing = carTypePricing;
        _logger = logger;
    }

    public Price CalculatePrice(Car car)
    {
        try
        {
            decimal price = 0;

            CarType carTypeEnum;

            if (!Enum.TryParse(car.CarType, out carTypeEnum))
                carTypeEnum = CarType.Other;

            price += _tirePricing.CalculateAdditionalTirePrice(car.TireSize);

            price += _carTypePricing.CalculateAdditionalCarTypePrice(carTypeEnum);

            //It is the simplest way but we may implement strategy pattern or if we have more options additional services add builder pattern, but know will be KISS
            price += car.IsWheelBalancingRequired ? WheelAlignmentPrice : 0;

            return new Price { Amount = price };
     
        }
        catch (Exception ex)
        {
            _logger.LogError(message: ex.Message, ex);
            throw;
        }
    }
}
