namespace Hedin.ChangeTires.Api.Services;

public class PricingService
{
    public decimal CalculatePrice(string carType, int tireSize, bool includeWheelAlignment)
    {
        decimal price = 0;

        if (carType == "Sedan")
        {
            price = 100;
        }
        else if (carType == "SUV")
        {
            price = 120;
        }
        else if (carType == "Truck")
        {
            price = 150;
        }
        else
        {
            price = 90;
        }

        if (tireSize <= 16)
        {
            price += 20;
        }
        else if (tireSize > 16 && tireSize <= 18)
        {
            price += 40;
        }
        else
        {
            price += 60;
        }
        
        if (includeWheelAlignment)
        {
            price += 50;
        }

        return price;
    }
}
