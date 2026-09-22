
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.Unicode;
using TripPlanner.DTO;

namespace TripPlanner.Services;
public class AmenitiesService
{
    private readonly HttpClient _client;
    private readonly string url = "https://overpass-api.de/api/interpreter";

    public AmenitiesService(HttpClient client)
    {
        _client = client;
    }

    public async Task<List<AmenityData>?> getAmenitiesData(List<Double> boundingBox, List<string> amenityRequest, List<GeoCodingData> geoCodeData)
    {
        string query = $"[bbox:{boundingBox[1]}, {boundingBox[0]},{boundingBox[3]}, {boundingBox[2]}][out:json][timeout:25]; (";
        foreach(string amenity in amenityRequest)
        {
            query += $"nwr[\"amenity\"=\"{amenity}\"];";
        }
        query+="); out center;";

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
            JsonNode overpassJson = JsonNode.Parse(responseBody)!;

            JsonArray elements = overpassJson["elements"]!.AsArray();
            List<AmenityData> amenityData = new List<AmenityData>();

            foreach(JsonNode? element in elements)
            {   
                if(element == null)
                {
                    continue;
                }
                AmenityData amenity = new AmenityData(element);
                
                List<double> resultFromHaversine = Haversine(geoCodeData, amenity.Lat, amenity.Lon);
                amenity.DistanceToStart = resultFromHaversine[0];
                amenity.DistanceToEnd = resultFromHaversine[1];

                amenityData.Add(amenity);
            }

            Console.WriteLine(amenityData[0].Lat);
            Console.WriteLine(amenityData[0].Lon);

            amenityData.Sort((a,b) => a.DistanceToStart.CompareTo(b.DistanceToStart));

            return amenityData;
           
        } 
        catch(HttpRequestException e)
        {
            Console.WriteLine(e.Message);
            return null;
        }
    }

    public List<double> Haversine(List<GeoCodingData> coordinatesToCalculateWith, double lat2, double lon2)
    {
        List<double> result = new List<double>();

        foreach(GeoCodingData data in coordinatesToCalculateWith)
        {
            double lat1 = double.Parse(data.lat);
            double lon1 = double.Parse(data.lon);

            double dLat = (Math.PI / 180) * (lat2 - lat1);
            double dLon = (Math.PI / 180) * (lon2 - lon1);

            double lat1Radians = (Math.PI / 180) * (lat1);
            double lat2Radians = (Math.PI / 180) * (lat2);

            double a = Math.Pow(Math.Sin(dLat / 2), 2) + 
                       Math.Pow(Math.Sin(dLon / 2), 2) * 
                       Math.Cos(lat1Radians) * Math.Cos(lat2Radians);
            
            double rad = 6371;
            double c = 2 * Math.Asin(Math.Sqrt(a));

            result.Add(rad * c);
        }

        return result;
    }

}
