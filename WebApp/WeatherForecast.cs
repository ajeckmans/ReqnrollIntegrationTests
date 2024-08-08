using System.Text.Json.Serialization;

namespace WebApp;

public record WeatherForecast
{
    public WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
    {
        this.Date = Date;
        this.TemperatureC = TemperatureC;
        this.Summary = Summary;
    }

    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    [JsonIgnore] public int Id { get; init; }

    public DateOnly Date { get; init; }
    public int TemperatureC { get; init; }
    public string? Summary { get; init; }
}