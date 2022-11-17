namespace AspNetCore.Common;

public class WeatherForecastService
{
    private static readonly string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    public static async Task<WeatherForecast[]> GetAsync(int count)
    {

        WeatherForecast[] forecast = await Task.Run(() => Enumerable.Range(1, count).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            )).ToArray());

        return forecast;
    }
}