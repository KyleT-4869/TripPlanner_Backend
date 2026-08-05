using Microsoft.AspNetCore.Mvc;
using TripPlanner.DTO;
using TripPlanner.Services;

namespace TripPlanner.Controller;
[Route("api/route")]
[ApiController]
public class RouteHandlingController : ControllerBase
{
    private readonly GeoCodingService _geoCodingService;
    private readonly RouteService _routeService;
    public RouteHandlingController(
        GeoCodingService geoCodingService,
        RouteService routeService
    )
    {
        _geoCodingService = geoCodingService;
        _routeService = routeService;
    }

    [HttpGet("getRoute")]
    public async Task<IActionResult> GetRoute([FromQuery] string origin, [FromQuery] string dest)
    {
        GeoCodingData? originGeoData = await _geoCodingService.GetGeoCodingData(origin);

        await Task.Delay(1000);

        GeoCodingData? destGeoData = await _geoCodingService.GetGeoCodingData(dest);

        RoutingData? routeData = await _routeService.GetRouteInformation(originGeoData!, destGeoData!);

        return Ok(routeData);

    }
}