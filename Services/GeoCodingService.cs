using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using System.Net.Http.Headers;

namespace TripPlanner.Services;
public class GeoCodingService
{
    private HttpClient client = new HttpClient();
    private const string NominatimUrl = "https://nominatim.openstreetmap.org/search?q=";
    private const string parameters = "&format=json&limit=1&addressdetails=1&countrycodes=us";

    public GeoCodingService()
    {
        client.DefaultRequestHeaders.UserAgent.ParseAdd(
        "TripPlanner/1.0 (kyletruong2000@gmail.com)"
        );
    }

    public async Task<string> CallNominatim(string startAddress, string destination) 
    {   
        string startUrl = $"{NominatimUrl}{startAddress}{parameters}";
        string destinationUrl = $"{NominatimUrl}{destination}{parameters}";
        Console.WriteLine(startUrl);
        Console.WriteLine(destinationUrl);

        using HttpResponseMessage StartResponse = await client.GetAsync(startUrl);

        await Task.Delay(1000);

        using HttpResponseMessage destinationResponse = await client.GetAsync(destinationUrl);

        var jsonResponseStart = await StartResponse.Content.ReadAsStringAsync();
        var jsonResponseDest = await destinationResponse.Content.ReadAsStringAsync();
        Console.WriteLine(jsonResponseStart);
        Console.WriteLine(jsonResponseDest);

        return "Hello";
    }
}