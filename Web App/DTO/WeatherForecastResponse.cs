namespace Web_App.DTO;

public class WeatherForecastResponse
{
    public DateOnly Date { get; set; }

    public int TemperatureC { get; set; }

    public int TemperatureF { get; set; }

    public string? Summary { get; set; }
}