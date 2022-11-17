using Grpc.Core;

namespace AspNetCoreGrpcService.Services;

public class WeatherForecastService : WeatherForecast.WeatherForecastBase
{
    private readonly ILogger<WeatherForecastService> _logger;
    public WeatherForecastService(ILogger<WeatherForecastService> logger)
    {
        _logger = logger;
    }

    public override async Task<WeatherForecastReply> GetWeatherForecasts(WeatherForecastRequest request, ServerCallContext context)
    {
        try
        {
            WeatherForecastReply wfr = new();
            AspNetCore.Common.WeatherForecast[] weatherForecasts = await AspNetCore.Common.WeatherForecastService.GetAsync(request.Count);
            IEnumerable<WeatherForecastItem> weatherForecastItems = weatherForecasts.Select(f => new WeatherForecastItem
            {
                Date = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(f.Date.ToDateTime(TimeOnly.MinValue).ToUniversalTime()),
                Summary = (Summaries)(int)f.Summary,
                SummaryString = f.SummaryString,
                TemperatureC = f.TemperatureC,
                IsCold = f.IsCold
            });
            wfr.WeatherForecasts.AddRange(weatherForecastItems);

            return wfr;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetWeatherForecasts error");
            return new WeatherForecastReply();
        }
    }
}