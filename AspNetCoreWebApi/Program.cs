using AspNetCore.Common;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/weatherforecast", async (WeatherForecastRequest request) =>
{
    var weatherForecasts = await WeatherForecastService.GetAsync(request.Count);
    return new WeatherForecastResponse(weatherForecasts.Select(f => new WeatherForecast(f.Date, f.TemperatureC, f.Summary, f.SummaryString, f.IsCold)).ToArray());
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();


