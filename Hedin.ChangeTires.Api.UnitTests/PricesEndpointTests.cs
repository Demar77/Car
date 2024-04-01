using Hedin.ChangeTires.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Hedin.ChangeTires.Api.Tests
{
    public class PricesEndpointTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public PricesEndpointTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Should_ReturnBadRequest_When_TireSizeOutOfRange()
        {
            // Arrange
            var client = _factory.CreateClient();
            Car car = new Car(new Guid(), "Sedan", 2, false);
            var jsonContent = new StringContent(JsonSerializer.Serialize(car), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync("/prices", jsonContent);

            // Assert

            var json = await response.Content.ReadAsStringAsync();
            var errorMessage = JsonDocument.Parse(json).RootElement[0].GetProperty("errorMessage").GetString();

            Assert.Equal(StatusCodes.Status400BadRequest, (int)response.StatusCode);
            Assert.Equal("Tire size must be between 12 to 25", errorMessage);
        }

        [Theory]
        [MemberData(nameof(Cars))]
        public async Task Should_CalculateCorrectPrice_ForAllVariationsCars(string url, string carType, int tireSize, bool includeWheelAlignment, decimal expectPrice)
        {
            // Arrange
            var client = _factory.CreateClient();
            Car car = new Car(new Guid(), carType, tireSize, includeWheelAlignment);
            var jsonContent = new StringContent(JsonSerializer.Serialize(car), Encoding.UTF8, "application/json");

            // Act
            var response = await client.PostAsync(url, jsonContent);

            // Assert
            response.EnsureSuccessStatusCode();
            var price = await response.Content.ReadFromJsonAsync<Price>();

            Assert.Equal(expectPrice, price?.Amount);
        }

        public static List<object[]> Cars = new()
        {
            new object[]{ "/prices","Sedan",15,false,120},
            new object[]{ "/prices","Sedan",16,false,120},
            new object[]{ "/prices","Sedan",17,false,140},
            new object[]{ "/prices","Sedan",18,false,140},
            new object[]{ "/prices","Sedan",19,false,160},
            new object[]{ "/prices","Sedan",20,false,160},
            new object[]{ "/prices","SUV",15,false,140},
            new object[]{ "/prices","SUV",16,false,140},
            new object[]{ "/prices","SUV",17,false,160},
            new object[]{ "/prices","SUV",18,false,160},
            new object[]{ "/prices","SUV",19,false,180},
            new object[]{ "/prices","SUV",20,false,180},
            new object[]{ "/prices","Truck",15,false,170},
            new object[]{ "/prices","Truck",16,false,170},
            new object[]{ "/prices","Truck",17,false,190},
            new object[]{ "/prices","Truck",18,false,190},
            new object[]{ "/prices","Truck",19,false,210},
            new object[]{ "/prices","Truck",20,false,210},
            new object[]{ "/prices","Other",15,false,110},
            new object[]{ "/prices","Other",16,false,110},
            new object[]{ "/prices","Other",17,false,130},
            new object[]{ "/prices","Other",18,false,130},
            new object[]{ "/prices","Other",19,false,150},
            new object[]{ "/prices","Other",20,false,150},
            new object[]{ "/prices","Sedan",15,true,170},
            new object[]{ "/prices","Sedan",16,true,170},
            new object[]{ "/prices","Sedan",17,true,190},
            new object[]{ "/prices","Sedan",18,true,190},
            new object[]{ "/prices","Sedan",19,true,210},
            new object[]{ "/prices","Sedan",20,true,210},
            new object[]{ "/prices","SUV",15,true,190},
            new object[]{ "/prices","SUV",16,true,190},
            new object[]{ "/prices","SUV",17,true,210},
            new object[]{ "/prices","SUV",18,true,210},
            new object[]{ "/prices","SUV",19,true,230},
            new object[]{ "/prices","SUV",20,true,230},
            new object[]{ "/prices","Truck",15,true,220},
            new object[]{ "/prices","Truck",16,true,220},
            new object[]{ "/prices","Truck",17,true,240},
            new object[]{ "/prices","Truck",18,true,240},
            new object[]{ "/prices","Truck",19,true,260},
            new object[]{ "/prices","Truck",20,true,260},
            new object[]{ "/prices","Other",15,true,160},
            new object[]{ "/prices","Other",16,true,160},
            new object[]{ "/prices","Other",17,true,180},
            new object[]{ "/prices","Other",18,true,180},
            new object[]{ "/prices","Other",19,true,200},
            new object[]{ "/prices","Other",20,true,200},
            new object[]{ "/prices","RandomStringFromUser",20,true,200},
        };
    }
}