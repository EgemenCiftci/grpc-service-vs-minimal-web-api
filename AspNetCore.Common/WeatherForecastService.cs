namespace AspNetCore.Common;

public class WeatherForecastService
{
    public static async Task<WeatherForecast[]> GetAsync(int count)
    {
        WeatherForecast[] forecast = await Task.Run(() => Enumerable.Range(1, count).Select(index =>
            new WeatherForecast
            (
                DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                Random.Shared.Next(-20, 55),
                (Summaries)Random.Shared.Next(9),
                ((Summaries)Random.Shared.Next(9)).ToString(),
                Convert.ToBoolean(Random.Shared.Next(2))
            )).ToArray());

        return forecast;
    }
}