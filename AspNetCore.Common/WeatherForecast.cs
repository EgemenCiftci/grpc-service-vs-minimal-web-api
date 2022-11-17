namespace AspNetCore.Common;

public record WeatherForecast(DateOnly Date, int TemperatureC, Summaries Summary, string? SummaryString, bool IsCold)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
