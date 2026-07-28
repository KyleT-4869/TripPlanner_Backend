using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace TripPlanner.DTO;

public class OpenRouteResult
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };
    public OpenRouteResult(JsonNode orsJson)
    {
        this.BoundingBox = orsJson["bbox"]!.Deserialize<List<double>>()!;
        this.Steps = orsJson["features"]![0]!["properties"]!["segments"]![0]!["steps"]!.Deserialize<List<Step>>(JsonOptions)!;
        this.Summary = orsJson["features"]![0]!["properties"]!["summary"]!.Deserialize<Summary>(JsonOptions)!;
        this.Geometry = orsJson["features"]![0]!["geometry"]!.Deserialize<Geometry>(JsonOptions)!;
    }
    public List<double> BoundingBox {get; set;} = [];
    public List<Step> Steps {get; set;} = [];
    public Summary Summary {get; set;}
    public Geometry Geometry {get; set;}

    

    public void Print()
    {
        Console.WriteLine($"Bounding Box: {BoundingBox[0]}");
        this.Steps[0].Print();
        this.Summary.Print();
        this.Geometry.Print();
    }

}

public class Step
{
    public double Distance {get; set;} = 0.0;
    public double Duration {get; set;} = 0.0;
    public int Type {get; set;} = 0;
    public string Instruction {get; set;} = ""; 

    [JsonPropertyName("way_points")]
    public List<int> Waypoint {get; set;} = [];

    public void Print()
    {
        Console.WriteLine($"Distance: {Distance}, Duration: {Duration}, Type: {Type}, Instruction: {Instruction}");
    }

}

public class Summary
{
    public double Distance {get; set;} = 0.0;
    public double Duration {get; set;} = 0.0;

    public void Print()
    {
        Console.WriteLine($"Distance: {Distance}, Duration: {Duration}");
    }
}

public class Geometry
{
    public string Type {get; set;} = "";
    public List<List<double>> Coordinates {get; set;} = [];

    public void Print()
    {
        Console.WriteLine($"Type: {Type}, Coordinates: {Coordinates[0][0]}");
    }
}