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
    return new WeatherForecastResponse(await WeatherForecastService.GetAsync(request.Count));
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();


