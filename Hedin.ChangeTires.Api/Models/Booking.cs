namespace Hedin.ChangeTires.Api.Models;

using System;

public class Booking
{
    public Booking(Guid id, DateTime date, User user, Car car)
    {
        Id = id;
        Date = date;
        Car = car;
        User = user;
    }

    // Private constructor for EF use
    private Booking()
    {
        // EF will use this constructor
    }

    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public Car Car { get; set; }
    public User User { get; set; }
    public Guid CarId { get; set; }
    public Guid UserId { get; set; }

    // Concurency check in real database
    // [Timestamp]
    //  public byte[] Version { get; set; }
}