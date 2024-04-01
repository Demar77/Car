using Microsoft.EntityFrameworkCore;

namespace Hedin.ChangeTires.Api
{
    public class CarContext : DbContext
    {
        public CarContext()
        { }

        public CarContext(DbContextOptions<CarContext> options) : base(options) { }

        public virtual DbSet<Car> Cars { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseInMemoryDatabase("CarDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
            .HasOne(b => b.Car)
            .WithMany()
            .HasForeignKey(c => c.CarId);

            modelBuilder.Entity<Booking>()
              .HasOne(b => b.User)
              .WithMany()
              .HasForeignKey(b => b.UserId);
        }
    }
}