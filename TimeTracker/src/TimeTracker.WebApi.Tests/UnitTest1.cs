using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using Xunit;

namespace TimeTracker.WebApi.Tests
{
    public class WeatherForecastControllerTests
    {
        [Fact]
        public async void GET_retrieves_weather_forecast()
        {
            //await using var application = new WebApplicationFactory<Api.Startup>();
            //using var client = application.CreateClient();

            //var response = await client.GetAsync("/weatherforecast");
            //response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}