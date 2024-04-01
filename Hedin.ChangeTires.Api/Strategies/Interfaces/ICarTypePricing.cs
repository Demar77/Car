using Hedin.ChangeTires.Api.Models.Enums;

namespace Hedin.ChangeTires.Api.Strategies.Interfaces
{
    public interface ICarTypePricing
    {
        decimal CalculateAdditionalCarTypePrice(CarType carType);
    }
}