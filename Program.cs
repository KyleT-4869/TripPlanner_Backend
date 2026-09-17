using TripPlanner.Services;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddHttpClient<GeoCodingService>(client =>
    {
        client.BaseAddress = new Uri("https://nominatim.openstreetmap.org/");
        client.DefaultRequestHeaders.UserAgent.ParseAdd("TripPlanner/1.0 (kyletruong2000@gmail.com)"); 
    }
);

builder.Services.AddHttpClient<RouteService>(client =>
{
    client.BaseAddress = new Uri("https://api.openrouteservice.org/");
});

builder.Services.AddControllers();


var app = builder.Build();
app.UseCors("FrontendPolicy");

app.MapGet("/", () => "Hello World!");
// app.MapGet("/amenity", async () =>
// {
//     using HttpClient client = new HttpClient();
//     client.DefaultRequestHeaders.UserAgent.ParseAdd("TripPlanner/1.0 (kyletruong2000@gmail.com)"); 
//     var service = new AmenitiesService(client);
//     List<Double> boundingBox = new List<Double>()
//     {
//         -118.372709,
//         34.110412,
//         -118.271662,
//         34.221766
//     };
//     List<string> amenityRequest = new List<string>()
//     {
//         "restaurant"
//     };

//     await service.getAmenitiesData(boundingBox, amenityRequest);
//     return Results.Ok();

// });
app.MapControllers();
app.Run();
