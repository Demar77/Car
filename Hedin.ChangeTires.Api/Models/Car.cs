namespace Hedin.ChangeTires.Api.Models
{
    public class Car
    {
        public Car(Guid id, string carType, int tireSize, bool isWheelBalancingRequired = false)
        {
            Id = id;
            CarType = carType;
            TireSize = tireSize;
            IsWheelBalancingRequired = isWheelBalancingRequired;
        }

        public Guid Id { get; set; }
        public string CarType { get; set; }
        public int TireSize { get; set; }
        public bool IsWheelBalancingRequired { get; set; }

        // Concurency check in real database
        // [Timestamp]
        //  public byte[] Version { get; set; }
    }
}