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
app.MapControllers();
app.Run();
