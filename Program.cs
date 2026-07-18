using TripPlanner.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<GeoCodingService>();


var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/callNominatim/start/{startAddress}/dest/{destination}", 
    (GeoCodingService geoService, string startAddress, string destination)
        => geoService.CallNominatim(startAddress, destination)
);
app.Run();
