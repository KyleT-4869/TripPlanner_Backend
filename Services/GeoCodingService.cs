using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Nodes;
using TripPlanner.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;

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

    public async Task<OpenRouteResult?> CallNominatim(string startAddress, string destination) 
    {   
        string startUrl = $"{nominatimUrl}{startAddress}{parameters}";
        string destinationUrl = $"{nominatimUrl}{destination}{parameters}";

        try
        {
            using HttpResponseMessage startUrlResponse = await client.GetAsync(startUrl);
            startUrlResponse.EnsureSuccessStatusCode();

            await Task.Delay(1000);

            using HttpResponseMessage destinationUrlResponse = await client.GetAsync(destinationUrl);
            destinationUrlResponse.EnsureSuccessStatusCode();

            List<NominatimResult>? startResult = await startUrlResponse.Content.ReadFromJsonAsync<List<NominatimResult>>();
            List<NominatimResult>? destResult = await destinationUrlResponse.Content.ReadFromJsonAsync<List<NominatimResult>>();

            NominatimResult start = startResult![0];
            NominatimResult dest = destResult![0];

            OpenRouteResult? orsResult = await callOpenRoute(start, dest);
            return orsResult;
        }
        catch(HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            return null;
            
        }
    }

    public async Task<OpenRouteResult?> callOpenRoute(NominatimResult startResult, NominatimResult destResult)
    {
        string orsUrl = $"{openRouteUrl}api_key={_apiKey}&start={startResult.lon},{startResult.lat}&end={destResult.lon},{destResult.lat}";
        try
        {
            using HttpResponseMessage response = await client.GetAsync(orsUrl);
            response.EnsureSuccessStatusCode();

            string stringResponse = await response.Content.ReadAsStringAsync();
            JsonNode orsJson = JsonNode.Parse(stringResponse)!;
            OpenRouteResult orsResult = new OpenRouteResult(orsJson);

            return orsResult;
        }
        catch(HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
}