using AspNetCore.Common;
using AspNetCoreGrpcService;
using Grpc.Net.Client;
using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;

bool useHttps = true;

Console.Write("Test starting in 10 seconds...");
await Task.Delay(1000 * 10);
Console.Write("Test started.");

Console.Write("Creating a grpc client...");
GrpcChannel channel = GrpcChannel.ForAddress(useHttps ? "https://localhost:7098" : "http://localhost:5073");
AspNetCoreGrpcService.WeatherForecast.WeatherForecastClient client0 = new(channel);
Console.WriteLine(" Success");
long totalMilliseconds0 = 0;
totalMilliseconds0 += await CallGrpc(client0, 1);
totalMilliseconds0 += await CallGrpc(client0, 10);
totalMilliseconds0 += await CallGrpc(client0, 100);
totalMilliseconds0 += await CallGrpc(client0, 1000);
totalMilliseconds0 += await CallGrpc(client0, 10000);
totalMilliseconds0 += await CallGrpc(client0, 100000);
double averageMilliseconds0 = totalMilliseconds0 / (double)(1 + 10 + 100 + 1000 + 10000 + 100000);
Console.WriteLine("Average ms per item: {0}", averageMilliseconds0);

Console.WriteLine();

Console.Write("Creating a http client...");
HttpClient client1 = new() { BaseAddress = new Uri(useHttps ? "https://localhost:7257" : "http://localhost:5031") };
Console.WriteLine(" Success");
long totalMilliseconds1 = 0;
totalMilliseconds1 += await CallWebApi(client1, 1);
totalMilliseconds1 += await CallWebApi(client1, 10);
totalMilliseconds1 += await CallWebApi(client1, 100);
totalMilliseconds1 += await CallWebApi(client1, 1000);
totalMilliseconds1 += await CallWebApi(client1, 10000);
totalMilliseconds1 += await CallWebApi(client1, 100000);
double averageMilliseconds1 = totalMilliseconds1 / (double)(1 + 10 + 100 + 1000 + 10000 + 100000);
Console.WriteLine("Average ms per item: {0}", averageMilliseconds1);

Console.ReadKey();

static async Task<long> CallGrpc(AspNetCoreGrpcService.WeatherForecast.WeatherForecastClient client, int count)
{
    Console.Write("Calling grpc service for {0} items...", count);

    Stopwatch sw = Stopwatch.StartNew();
    WeatherForecastReply response = await client.GetWeatherForecastsAsync(new AspNetCoreGrpcService.WeatherForecastRequest { Count = count });
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

    return sw.ElapsedMilliseconds;
}

static async Task<long> CallWebApi(HttpClient client, int count)
{
    Console.Write("Calling web api for {0} items...", count);

    Stopwatch sw = Stopwatch.StartNew();
    HttpResponseMessage response = await client.PostAsync($"weatherforecast?count={count}", JsonContent.Create(new AspNetCore.Common.WeatherForecastRequest(count)));
    sw.Stop();

    bool isSuccess = response.StatusCode == HttpStatusCode.OK && (await response.Content.ReadFromJsonAsync<WeatherForecastResponse>())?.WeatherForecasts?.Length == count;
    if (isSuccess)
    {
        Console.WriteLine(" Success: {0}ms", sw.ElapsedMilliseconds);
    }
    else
    {
        Console.WriteLine(" Error");
    }

    return sw.ElapsedMilliseconds;
}