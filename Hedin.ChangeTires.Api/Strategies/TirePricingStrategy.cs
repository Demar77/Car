using Hedin.ChangeTires.Api.Models.Enums;
using Hedin.ChangeTires.Api.Strategies.Interfaces;

namespace Hedin.ChangeTires.Api.Strategies
{
    public class SmallTirePricingStrategy : ITirePricingStrategy
    {
        private const decimal SmallTirePrice = 20m;

        public decimal GetPrice(int tireSize) => SmallTirePrice;
    }

    public class MediumTirePricingStrategy : ITirePricingStrategy
    {
        private const decimal MediumTirePrice = 40m;

        public decimal GetPrice(int tireSize) => MediumTirePrice;
    }

    public class LargeTirePricingStrategy : ITirePricingStrategy
    {
        private const decimal LargeTirePrice = 60m;

        public decimal GetPrice(int tireSize) => LargeTirePrice;
    }

    public class TirePricing : ITirePricing
    {
        private const int MaxSmallTireSize = 16;
        private const int MaxMediumTireSize = 18;

        private readonly Dictionary<TireSize, ITirePricingStrategy> _pricingStrategies;

        public TirePricing()
        {
            _pricingStrategies = new Dictionary<TireSize, ITirePricingStrategy>
        {
            { TireSize.Small, new SmallTirePricingStrategy() },
            { TireSize.Medium, new MediumTirePricingStrategy() },
            { TireSize.Large, new LargeTirePricingStrategy() }
        };
        }

        public decimal CalculateAdditionalTirePrice(int tireSize)
        {
            var sizeCategory = tireSize <= MaxSmallTireSize ? TireSize.Small :
                               tireSize <= MaxMediumTireSize ? TireSize.Medium :
                               TireSize.Large;
            return _pricingStrategies[sizeCategory].GetPrice(tireSize);
        }
    }
}