
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Unicode;

namespace TripPlanner.Services;
public class AmenitiesService
{
    private readonly HttpClient _client;
    private readonly string url = "https://overpass-api.de/api/interpreter";

    public AmenitiesService(HttpClient client)
    {
        _client = client;
    }

    public async Task getAmenitiesData(List<Double> boundingBox, List<string> amenityRequest)
    {
        string query = $"[bbox:{boundingBox[1]}, {boundingBox[0]},{boundingBox[3]}, {boundingBox[2]}][out:json][timeout:25];";
        foreach(string amenity in amenityRequest)
        {
            query += $"nwr[\"amenity\"=\"{amenity}\"];";
        }
        query+="out geom;";

        var formData = new Dictionary<string, string>()
        {
          ["data"] = query  
        };

        FormUrlEncodedContent queryToSend = new FormUrlEncodedContent(formData);

        try
        {
            using HttpResponseMessage response = await _client.PostAsync(url, queryToSend);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            await File.WriteAllTextAsync(
                "overpass-response.json",
                responseBody
            );
        } 
        catch(HttpRequestException e)
        {
            Console.WriteLine(e.Message);
        }
    }

}
