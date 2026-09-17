using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace TripPlanner.DTO;
public class AmenityData
{
    public double Lat {get; set;} = 0.0;
    public double Lon {get; set;} = 0.0;
    public string Address {get; set;} =" ";

    public double DistanceToStart {get; set;} = 0.0;

    public double DistanceToEnd {get; set;} = 0.0;
    public Tags Tags {get; set;} 

    public AmenityData(JsonNode overpassJson)
    {
        string type = overpassJson["type"]!.Deserialize<string>()!;

        if(type == "node")
        {
            this.Lat = overpassJson["lat"]!.Deserialize<double>()!;
            this.Lon = overpassJson["lon"]!.Deserialize<double>()!;
        }

        else if(type == "way")
        {
            this.Lat = overpassJson["center"]!["lat"]!.Deserialize<double>()!;
            this.Lon = overpassJson["center"]!["lon"]!.Deserialize<double>()!;
        }

        this.Tags = new Tags();
        this.Tags.name = overpassJson["tags"]!["name"]?.Deserialize<string>()!;
        this.Tags.amenity = overpassJson["tags"]!["amenity"]?.Deserialize<string>()!;
        this.Tags.phone = overpassJson["tags"]!["phone"]?.Deserialize<string>()!;
        this.Tags.website = overpassJson["tags"]!["website"]?.Deserialize<string>()!;
        this.Tags.cashPayment = overpassJson["tags"]!["payment:cash"]?.Deserialize<string>();
        this.Tags.cardPayment = overpassJson["tags"]!["payment:debit_cards"]?.Deserialize<string>()!;
        this.Tags.houseNumber = overpassJson["tags"]!["addr:housenumber"]?.Deserialize<string>()!;
        this.Tags.street = overpassJson["tags"]!["addr:street"]?.Deserialize<string>()!;
        this.Tags.unit = overpassJson["tags"]!["addr:unit"]?.Deserialize<string>()!;
        this.Tags.city = overpassJson["tags"]!["addr:city"]?.Deserialize<string>()!;
        this.Tags.state = overpassJson["tags"]!["addr:state"]?.Deserialize<string>()!;
        this.Tags.postcode = overpassJson["tags"]!["addr:postcode"]?.Deserialize<string>()!;
        
    }
}

public class Tags
{
    
    public string name {get; set;} = " ";

    public string amenity {get; set;} = " ";

    public string phone {get; set;} =" ";

    public string? website {get; set;} = " ";

    [JsonPropertyName("payment:cash")]
    public string? cashPayment {get; set;} = " ";
    [JsonPropertyName("payment:debit_cards")]
    public string cardPayment {get; set;} = " ";

    [JsonPropertyName("addr:housenumber")]
    public string? houseNumber {get; set;} =" ";

    [JsonPropertyName("addr:street")]
    public string? street {get; set;} =" ";

    [JsonPropertyName("addr:unit")]
    public string? unit {get; set;} = " ";

    [JsonPropertyName("addr:city")]
    public string? city {get; set;} =" ";

    [JsonPropertyName("addr:state")]
    public string? state {get; set;} = " ";

    [JsonPropertyName("addr:postcode")]
    public string? postcode {get; set;} =" ";

}