using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Hedin.ChangeTires.Api.Services;
using Hedin.ChangeTires.Api.ExternalIntegrations;
using Hedin.ChangeTires.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<ExternalServiceSettings>(builder.Configuration.GetSection("ExternalServiceSettings"));

builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<ExternalServiceSettings>>().Value);

builder.Services.AddHttpClient<ExternalSlotApiClient>((serviceProvider, client) =>
{
    var settings = serviceProvider.GetRequiredService<ExternalServiceSettings>();
    client.BaseAddress = new Uri(settings.Url);
});

var app = builder.Build();

var externalServiceSettings = app.Services.GetRequiredService<ExternalServiceSettings>();

var pricingService = new PricingService();
var bookingService = new BookingService(externalServiceSettings);

app.MapPost("/bookings", (DateTime slot) => {
    var result = bookingService.BookTireChange(slot);
    return Results.Ok("Booking successful: " + result);
});

app.MapGet("/prices", (string carType, int tireSize, bool includeWheelAlignment) => {
    var price = pricingService.CalculatePrice(carType, tireSize, includeWheelAlignment);
    return Results.Ok("Price calculated: " + price);
});

app.MapGet("/slots", (ExternalSlotApiClient slotClient) => {
    var slots = slotClient.GetAvailableSlots();
    return Results.Ok("Slots retrieved: " + slots);
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseHttpsRedirection();

app.Run();
