using TripPlanner.DTO;

namespace TripPlanner.Services;
public class GeoCodingService
{
    private readonly HttpClient _client;
    private const string parameters = "&format=json&limit=1&addressdetails=1&countrycodes=us";



    public GeoCodingService(HttpClient client)
    {
        _client = client;
    }


    public async Task<GeoCodingData?> GetGeoCodingData(string address)
    {
        string addressUrl = $"search?q={address}{parameters}";
        
        try
        {
            using HttpResponseMessage response = await _client.GetAsync(addressUrl);
            response.EnsureSuccessStatusCode();

            List<GeoCodingData>? listGeoCodingData = await response.Content.ReadFromJsonAsync<List<GeoCodingData>>();
            GeoCodingData GeoCodingData = listGeoCodingData![0];

            return GeoCodingData;
        } 
        catch(HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

}