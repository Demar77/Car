using Hedin.ChangeTires.Api.Models.Enums;

namespace Hedin.ChangeTires.Api.Strategies.Interfaces
{
    public interface ICarTypePricingStrategy
    {
        decimal GetPrice(CarType carType);
    }
}