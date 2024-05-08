using Garage3._0.Services;
using Microsoft.AspNetCore.Mvc;

namespace Garage3._0.Controllers;

public class VehiclesController : Controller
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }

    // GET: Vehicles/ParkedOverview
    public async Task<IActionResult> Index(string? searchQuery = null)
    {

        Console.WriteLine("did i hit the endpoint?");
        // Fetch parked vehicle view models from the service
        var viewModel = await _vehicleService.GetVehiclesIndexViewModelAsync(searchQuery);

        Console.WriteLine("after viewmodel fetch");

        foreach (var vehicleViewModel in viewModel.Vehicles) {
            Console.WriteLine(vehicleViewModel.LicensePlate);
        }
        // Pass the data to the view
        return View(viewModel);
    }
}