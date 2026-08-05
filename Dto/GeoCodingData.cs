
using System;
namespace TripPlanner.DTO;

public class GeoCodingData
{
    public string lon {get; set;} = " ";
    public string lat{get; set;} = " ";

    public void Print()
    {
        Console.WriteLine(this.lon);
        Console.WriteLine(this.lat);
    }   
}