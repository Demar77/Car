using Hedin.ChangeTires.Api.Configurations;
using Hedin.ChangeTires.Api.ExternalIntegrations;
using Hedin.ChangeTires.Api.Services;
using Hedin.ChangeTires.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Text;

namespace Hedin.ChangeTires.Api
{
    /// <summary>
    /// Class helps webapi for more readable and management. 
    /// </summary>
    public static class CarRequests
    {
        private static readonly Serilog.ILogger _log = Log.ForContext(typeof(Program));
        /// <summary>
        /// An extension method allows you to add new functionality to an existing type (class, interface, or struct) without modifying its source code.
        /// </summary>
        /// <param name="app">WebApplication </param>
        /// <returns></returns>
        public static WebApplication RegisterEndpoints(this WebApplication app)
        {
            app.MapPost("/bookings", CarRequests.BookTireChange)
                .Produces<string?>()
                .Produces<string?>(StatusCodes.Status404NotFound)
                .WithTags("Booking")
                .RequireAuthorization();

            app.MapPost("/prices", CarRequests.CalculatePrice)
                .Produces(StatusCodes.Status400BadRequest)
                .Produces<Car>()
                .Accepts<Car>("application/json")
                .WithValidator<Car>()
                .WithTags("Car");

            app.MapGet("/slots", CarRequests.GetBookedSlots)
                .Produces(StatusCodes.Status404NotFound)
                .Produces<List<Booking>>()
                .WithTags("Booking")
                .RequireAuthorization();

            app.MapGet("/slotsext", CarRequests.GetHiddenExternalApi)
                 .Produces<List<Booking>>()
                 .Produces(StatusCodes.Status404NotFound)
                 .WithTags("Booking")
                 .ExcludeFromDescription();

            app.MapPost("/slotsext", CarRequests.PostHiddenExternalApi)
                .Produces<Booking>()
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithTags("Booking")
                .ExcludeFromDescription();

            app.MapPost("/login", CarRequests.GetToken)
                .Produces<string?>()
                .Produces<string?>(StatusCodes.Status401Unauthorized);

            return app;
        }

        public static IResult GetToken(TokenRequest request, [FromServices] IConfiguration configuration)
        {
            if (IsValidUser(request.Login, request.Password))
            {

                var claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, "user-id"),
                    new Claim(ClaimTypes.Name, "Marcin"),
                    new Claim(ClaimTypes.Role, "Admin"),
                };

                var token = new JwtSecurityToken
                (
                    issuer: configuration["JwtIssuer"],
                    audience: configuration["JwtIssuer"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(60),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtKey"])),
                        SecurityAlgorithms.HmacSha256)
                );

                var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
                return Results.Ok(jwtToken);
            }
            return Results.StatusCode(StatusCodes.Status401Unauthorized);
        }

        public static IResult BookTireChange(DateTime slot, IBookingService service, ExternalServiceSettings externalServiceSettings, HttpContext context, ClaimsPrincipal user)
        {
            try
            {
                //We have token and we may do any think we want :)
                string token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                //We have all information about user
                var userName = user.Identity.Name;

                service = new BookingService(externalServiceSettings);

                _log.Information("Example log in static class CarRequests");
                _log.Information($">>> Loged user name is {userName} <<<");

                return service.BookTireChange(slot);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);

                throw;
            }
        }

        public static IResult CalculatePrice(IPricingService service, [FromBody] Car car)
        {
            return Results.Ok(service.CalculatePrice(car));
        }

        public static IResult GetBookedSlots(ExternalSlotApiClient slotClient, IExternalSlotApiClient service)
        {
            var slots = service.GetBookedSlots();

            if (slots == null)
            {
                return Results.NotFound();
            }

            return Results.Ok(slots);
        }

        public static IResult GetHiddenExternalApi(CarContext carContext)
        {
            CreateDefaultSlots(carContext);

            var allSlots = carContext.Bookings.Include(p => p.Car).Include(p => p.User).ToList();
            if (allSlots == null || allSlots.Count == 0)
            {
                return Results.NotFound();
            }

            return Results.Ok(allSlots);
        }

        public static IResult PostHiddenExternalApi(CarContext carContext, object slot)
        {
            CreateDefaultSlots(carContext);

            if (DateTime.TryParseExact(slot.ToString(), "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
            {
                if (IsFreeSlot(carContext, result, out Booking? slotResult))
                {
                    //Logic: if we don't have date we create it we get information about confirm reservation
                    //Car and Customer name we will be have from proces Authorization
                    var user = new User(new Guid(), "Marcin");
                    var car = new Car(new Guid(), "Sedan", 15, false);

                    if (user == null || car == null)
                    {
                        return Results.BadRequest();
                    }

                    CreateReservation(carContext, result, car, user);

                    return Results.Ok(slotResult);
                }
                return Results.NotFound();
            }
            else
            {
                return Results.BadRequest();
            }
        }

        private static void CreateDefaultSlots(CarContext carContext)
        {
            if (carContext.Bookings.FirstOrDefault() == null)
            {
                Car sedan = new Car(new Guid("257dd2d6-a7e9-4c66-8d52-6d68e82bf959"), "Sedan", 15);
                Car truck = new Car(new Guid("c30a688f-98a1-4d5d-a92c-02314305a431"), "Truck", 21);
                Car other = new Car(new Guid("2f704d48-151e-4b2a-835b-e8d49ad40a0f"), "Other", 14);

                User john = new User(new Guid("24cc4117-0c84-4edc-8c4d-f6faa3693eb9"), "John");
                User adam = new User(new Guid("932ae47a-5426-4fe4-ae5b-8bb176c6cf4d"), "Adam");
                User eva = new User(new Guid("79908eac-ea44-43b8-b6a7-ba8966ad6e12"), "Eva");

                carContext.Users.Add(john);
                carContext.Users.Add(adam);
                carContext.Users.Add(eva);

                carContext.Cars.Add(sedan);
                carContext.Cars.Add(truck);
                carContext.Cars.Add(other);
                carContext.SaveChanges();

                carContext.Bookings.Add(new Booking(new Guid("7139f3de-cbfd-4feb-b7a1-c1bb5f16635e"), new DateTime(2010, 1, 1), john, sedan));
                carContext.Bookings.Add(new Booking(new Guid("e7543afe-2765-4cd9-9780-03b07f8f40ef"), new DateTime(2010, 1, 2), adam, truck));
                carContext.Bookings.Add(new Booking(new Guid("2ea925c8-d6db-4fcb-81d7-a066102188ea"), new DateTime(2010, 1, 3), eva, other));
                carContext.SaveChanges();

                carContext.SaveChanges();
            }
        }

        private static void CreateReservation(CarContext carContext, DateTime date, Car car, User user)
        {
            carContext.Bookings.Add(new Booking(Guid.NewGuid(), date, user, car));
            carContext.SaveChanges();
        }

        private static bool IsFreeSlot(CarContext carContext, DateTime date, out Booking? slot)
        {
            slot = carContext.Bookings.FirstOrDefault(p => p.Date == date);
            return slot == null;
        }
        private static bool IsValidUser(string login, string password)
        {
            // Place user verification logic here (e.g., a database check)
            // Return true if the user is valid, or false otherwise
            // Example:
            return login == "Marcin" && password == "mypassword";
        }
    }
}