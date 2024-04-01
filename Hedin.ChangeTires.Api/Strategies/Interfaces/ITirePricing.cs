namespace Hedin.ChangeTires.Api.Strategies.Interfaces
{
    public interface ITirePricing
    {
        decimal CalculateAdditionalTirePrice(int tireSize);
    }
}