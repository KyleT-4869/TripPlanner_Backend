using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using TripPlanner.DTO;

namespace TripPlanner.Services;
public class GeoCodingService
{
    private HttpClient client = new HttpClient();
    private const string nominatimUrl = "https://nominatim.openstreetmap.org/search?q=";
    private const string openRouteUrl = "https://api.openrouteservice.org/v2/directions/driving-car?";
    private const string parameters = "&format=json&limit=1&addressdetails=1&countrycodes=us";

    private readonly string _apiKey;


    public GeoCodingService(IConfiguration config)
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd(
        "TripPlanner/1.0 (kyletruong2000@gmail.com)"
        );

        _apiKey = config["ORS:ApiKey"]!;
    }

    public async Task<string> CallNominatim(string startAddress, string destination) 
    {   
        string startUrl = $"{nominatimUrl}{startAddress}{parameters}";
        string destinationUrl = $"{nominatimUrl}{destination}{parameters}";
        Console.WriteLine(startUrl);
        Console.WriteLine(destinationUrl);

        using HttpResponseMessage StartResponse = await client.GetAsync(startUrl);
        await Task.Delay(1000);
        using HttpResponseMessage destinationResponse = await client.GetAsync(destinationUrl);

        List<NominatimResult>? startResult = await StartResponse.Content.ReadFromJsonAsync<List<NominatimResult>>();
        List<NominatimResult>? destResult = await destinationResponse.Content.ReadFromJsonAsync<List<NominatimResult>>();
        

        NominatimResult start = startResult![0];
        NominatimResult dest = destResult![0];

        await callOpenRoute(start, dest);

        return "Hello";
    }

    public async Task<string> callOpenRoute(NominatimResult startResult, NominatimResult destResult)
    {
        string orsUrl = $"{openRouteUrl}api_key={_apiKey}&start={startResult.lon},{startResult.lat}&end={destResult.lon},{destResult.lat}";

        using HttpResponseMessage response = await client.GetAsync(orsUrl);
        string stringResponse = await response.Content.ReadAsStringAsync();
        JsonNode orsJson = JsonNode.Parse(stringResponse)!;
        OpenRouteResult orsResult = new OpenRouteResult(orsJson);
        orsResult.Print();


        return "Hello";
    }
}