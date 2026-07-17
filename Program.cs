using TripPlanner.Services;
GeoCodingService geoService = new GeoCodingService();
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/callNominatim/start/{startAddress}/dest/{destination}", geoService.CallNominatim);
app.Run();
