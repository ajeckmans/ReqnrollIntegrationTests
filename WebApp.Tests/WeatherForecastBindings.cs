using System.Text.Json;

using FluentAssertions;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;

using WebApp.Tests.Support;

namespace WebApp.Tests;

[Binding]
public class WeatherForecastBindings(WebApplicationContext webAppContext, DatabaseInformation databaseInformation)
{
    [Given(@"I have a weather forecast")]
    public void GivenIHaveAWeatherForecast(Table table)
    {
        var weatherForecasts = table.CreateSet<Models.WeatherForecast>().ToList();
        
        using var sqlConnection = new SqlConnection(databaseInformation.ConnectionString);
        sqlConnection.Open();
        
        using var sqlCommand = sqlConnection.CreateCommand();
        foreach (Models.WeatherForecast weatherForecast in weatherForecasts)
        {
            sqlCommand.CommandText = "INSERT INTO WeatherForecasts (Date, TemperatureC, Summary) VALUES (@Date, @TemperatureC, @Summary)";
            sqlCommand.Parameters.Clear();
            sqlCommand.Parameters.AddWithValue("@Date", weatherForecast.Date);
            sqlCommand.Parameters.AddWithValue("@TemperatureC", weatherForecast.TemperatureC);
            sqlCommand.Parameters.AddWithValue("@Summary", weatherForecast.Summary);
            sqlCommand.ExecuteNonQuery();
        }
    }

    [When(@"I request the weather forecast")]
    public async Task WhenIRequestTheWeatherForecastFor()
    {
        await webAppContext.GetAsync("/weatherforecast");
    }

    [Then(@"I should receive a weather forecast")]
    public void ThenIShouldReceiveAWeatherForecast(Table table)
    {
        var expectedWeatherForecasts = table.CreateSet<Models.WeatherForecast>().ToList();

        var deserializationOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };
        
        var response = JsonSerializer.Deserialize<Models.WeatherForecast[]>(webAppContext.BodyOfLastResponse!, deserializationOptions);

        response.Should().BeEquivalentTo(expectedWeatherForecasts);
    }
}