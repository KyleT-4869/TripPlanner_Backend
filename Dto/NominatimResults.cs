
using System;
namespace TripPlanner.DTO;

public class NominatimResult
{
    public string lon {get; set;} = " ";
    public string lat{get; set;} = " ";

    public void Print()
    {
        Console.WriteLine(this.lon);
        Console.WriteLine(this.lat);
    }   
}