using System.Text.Json.Nodes;
using TripPlanner.DTO;

namespace TripPlanner.Services;

public class RouteService {
    
    private readonly HttpClient _client;
    private readonly string _apiKey;
    

    public RouteService(IConfiguration config, HttpClient client)
    {
        _client = client;
        _apiKey = config["ORS:ApiKey"]!;
    }

    public async Task<RoutingData?> GetRouteInformation(GeoCodingData origin, GeoCodingData dest)
    {
        string orsUrl = $"v2/directions/driving-car" +
                        $"?api_key={_apiKey}" +
                        $"&start={origin.lon},{origin.lat}" +
                        $"&end={dest.lon},{dest.lat}";
        try
        {
            using HttpResponseMessage response = await _client.GetAsync(orsUrl);
            response.EnsureSuccessStatusCode();

            string stringResponse = await response.Content.ReadAsStringAsync();
            JsonNode orsJson = JsonNode.Parse(stringResponse)!;
            RoutingData orsResult = new RoutingData(orsJson);

            return orsResult;
        }
        catch(HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }
    
}