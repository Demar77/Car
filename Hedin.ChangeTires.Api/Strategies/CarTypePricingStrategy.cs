using Hedin.ChangeTires.Api.Models.Enums;
using Hedin.ChangeTires.Api.Strategies.Interfaces;

namespace Hedin.ChangeTires.Api.Strategies
{
    public class SedanPricingStrategy : ICarTypePricingStrategy
    {
        private const decimal SedanPrice = 100m;

        public decimal GetPrice(CarType carType) => SedanPrice;
    }

    public class SuvPricingStrategy : ICarTypePricingStrategy
    {
        private const decimal SuvPrice = 120m;

        public decimal GetPrice(CarType carType) => SuvPrice;
    }

    public class TruckPricingStrategy : ICarTypePricingStrategy
    {
        private const decimal TruckPrice = 150m;

        public decimal GetPrice(CarType carType) => TruckPrice;
    }

    public class OtherPricingStrategy : ICarTypePricingStrategy
    {
        private const decimal OtherPrice = 90m;

        public decimal GetPrice(CarType carType) => OtherPrice;
    }

    public class CarTypePricing : ICarTypePricing
    {
        private readonly Dictionary<CarType, ICarTypePricingStrategy> _pricingStrategies;

        public CarTypePricing()
        {
            _pricingStrategies = new Dictionary<CarType, ICarTypePricingStrategy>
        {
            { CarType.Sedan, new SedanPricingStrategy() },
            { CarType.SUV, new SuvPricingStrategy() },
            { CarType.Truck, new TruckPricingStrategy() },
            { CarType.Other, new OtherPricingStrategy() },
        };
        }

        public decimal CalculateAdditionalCarTypePrice(CarType carType)
        {
            return _pricingStrategies[carType].GetPrice(carType);
        }
    }
}