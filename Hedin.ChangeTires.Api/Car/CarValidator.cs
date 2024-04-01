using FluentValidation;

namespace Hedin.ChangeTires.Api
{
    public class CarValidator : AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(t => t.TireSize)
                .InclusiveBetween(12, 25)
                .WithMessage("Tire size must be between 12 to 25");
        }
    }
}