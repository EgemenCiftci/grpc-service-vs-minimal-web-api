using AspNetCoreGrpcService;
using Grpc.Net.Client;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

Console.Write("Creating a grpc client...");
//GrpcChannel channel = GrpcChannel.ForAddress("http://localhost:5073");
GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:7098");
WeatherForecast.WeatherForecastClient client0 = new(channel);
Console.WriteLine(" Success");
await CallGrpc(client0, 10);
await CallGrpc(client0, 100);
await CallGrpc(client0, 1000);
await CallGrpc(client0, 10000);
await CallGrpc(client0, 100000);

Console.WriteLine();

Console.Write("Creating a http client...");
//HttpClient client = new() { BaseAddress = new Uri("http://localhost:5031") };
HttpClient client1 = new() { BaseAddress = new Uri("https://localhost:7257") };
Console.WriteLine(" Success");
await CallWebApi(client1, 10);
await CallWebApi(client1, 100);
await CallWebApi(client1, 1000);
await CallWebApi(client1, 10000);
await CallWebApi(client1, 100000);

Console.ReadKey();

static async Task CallGrpc(WeatherForecast.WeatherForecastClient client, int count)
{
    Console.Write("Calling grpc service for {0} items...", count);
    Stopwatch sw = Stopwatch.StartNew();
    WeatherForecastReply response = await client.GetWeatherForecastsAsync(new WeatherForecastRequest { Count = count });
    sw.Stop();
    bool isSuccess = response?.WeatherForecasts.Count == count;
    if (isSuccess)
    {
        Console.WriteLine(" Success: {0}ms", sw.ElapsedMilliseconds);
    }
    else
    {
        Console.WriteLine(" Error");
    }
}

static async Task CallWebApi(HttpClient client, int count)
{
    Console.Write("Calling web api for {0} items...", count);
    Stopwatch sw = Stopwatch.StartNew();
    HttpResponseMessage response = await client.PostAsync($"weatherforecast?count={count}", JsonContent.Create(new WeatherForecastRequest { Count = count }));
    sw.Stop();
    bool isSuccess = response.StatusCode == System.Net.HttpStatusCode.OK && (await JsonSerializer.DeserializeAsync<AspNetCore.Common.WeatherForecast[]>(await response.Content.ReadAsStreamAsync()))?.Length == count;
    if (isSuccess)
    {
        Console.WriteLine(" Success: {0}ms", sw.ElapsedMilliseconds);
    }
    else
    {
        Console.WriteLine(" Error");
    }
}