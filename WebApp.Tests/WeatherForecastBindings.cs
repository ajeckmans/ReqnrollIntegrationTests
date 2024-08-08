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
        // Convert the table to a list of weather forecasts so we can insert them into the database
        var weatherForecasts = table.CreateSet<Models.WeatherForecast>().ToList();
        
        
        // Here we're going to insert the weather forecasts into the running test container database
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
        // we execute the request to the web app and the response is stored in the webAppContext itself
        await webAppContext.GetAsync("/weatherforecast");
    }

    [Then(@"I should receive a weather forecast")]
    public void ThenIShouldReceiveAWeatherForecast(Table table)
    {
        // Convert the table to a list of weather forecasts so we can compare them to the response
        var expectedWeatherForecasts = table.CreateSet<Models.WeatherForecast>().ToList();

        // Deserialize the response from the web app
        var response = JsonSerializer.Deserialize<Models.WeatherForecast[]>(webAppContext.BodyOfLastResponse!, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // Compare the response to the expected weather forecasts
        response.Should().BeEquivalentTo(expectedWeatherForecasts);
    }
}