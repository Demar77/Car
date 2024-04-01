namespace Hedin.ChangeTires.Api.Strategies.Interfaces
{
    public interface ITirePricingStrategy
    {
        decimal GetPrice(int tireSize);
    }
}