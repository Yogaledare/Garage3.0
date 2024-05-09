using Garage3._0.Data;
using Garage3._0.Filters;
using Garage3._0.Models;
using Garage3._0.Models.ViewModels;
using Garage3._0.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding; // For ModelState
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // For View


namespace Garage3._0.Controllers;

public class MembersController : Controller {
    private readonly GarageDbContext _context;
    private readonly IMemberService _memberService;
    private readonly IVehicleService _vehicleService;

    public MembersController(GarageDbContext context, IMemberService memberService, IVehicleService vehicleService) {
        _context = context;
        _memberService = memberService;
        _vehicleService = vehicleService;
    }


    public async Task<IActionResult> Index(string? searchQuery = null) {
        var model = await _memberService.GetMembersIndexViewModelAsync(searchQuery);
        return View(model);
    }

    // GET: Members/Create
    public IActionResult CreateMember() {
        return View();
    }

    // POST: Members/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ModelStateIsValid]
    public IActionResult CreateMember(CreateMemberViewModel input) {
        var member = new Member {
            SocialSecurityNr = input.SocialSecurityNr,
            Firstname = input.Firstname,
            Surname = input.Surname,
        };

        _memberService.RegisterMember(member);
        return RedirectToAction(nameof(Index));
    }


    public IActionResult CreateVehicleType(int memberId) {
        var model = new CreateVehicleTypeViewModel {
            MemberId = memberId
        };

        return View(model);
    }


    [HttpPost]
    [ModelStateIsValid]
    public IActionResult CreateVehicleType(CreateVehicleTypeViewModel model) {
        var vehicleType = new VehicleType {
            VehicleTypeName = model.VehicleTypeName,
            ParkingSpaceRequirement = model.ParkingSpaceRequirement
        };

        foreach (var wheels in model.AllowedWheelNumbers) {
            vehicleType.WheelConfigurations.Add(new WheelConfiguration {
                NumWheels = wheels
            });
        }

        _context.VehicleTypes.Add(vehicleType);
        _context.SaveChanges();

        return RedirectToAction("CreateVehicle", "Members", new {memberId = model.MemberId});
    }


    // GET: Members/5/RegisterVehicle
    [HttpGet("Members/{memberId:int}/CreateVehicle")]
    [ModelStateIsValid]
    public IActionResult CreateVehicle(int memberId) {
        var vehicleTypes = _vehicleService.GetVehicleTypeOptions();

        var model = new CreateVehicleViewModel {
            MemberId = memberId,
            VehicleTypeOptions = vehicleTypes
        };

        if (!vehicleTypes.Any()) {
            // Log the issue or handle the case where no vehicle types are available
            ModelState.AddModelError("", "No vehicle types available.");
        }

        return View(model);
    }


    [HttpPost("Members/{memberId:int}/CreateVehicle")]
    [ValidateAntiForgeryToken]
    public IActionResult CreateVehicle(CreateVehicleViewModel input) {
        if (ModelState.IsValid) {
            Console.WriteLine(input.LicencePlate);
            Console.WriteLine(input.Color);
            Console.WriteLine(input.Brand);
            Console.WriteLine(input.Model);
            Console.WriteLine(input.NumWheels);
            Console.WriteLine(input.MemberId);
            Console.WriteLine(input.VehicleTypeId);

            var vehicle = new Vehicle {
                LicencePlate = input.LicencePlate,
                Color = input.Color,
                Brand = input.Brand,
                Model = input.Model,
                NumWheels = input.NumWheels ?? 0,
                VehicleTypeId = input.VehicleTypeId ?? 0,
                MemberId = input.MemberId ?? 0,
            };

            _context.Vehicles.Add(vehicle);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // I cant use the [ModelStateIsValid]-filter on this action method.
        // The form will forget the list of vehicle types, and I will have nowhere to repopulate it if i use the filter. 
        // If anyone reading this has a solution, please let me know! 
        input.VehicleTypeOptions = _vehicleService.GetVehicleTypeOptions();
        return View(input);
    }
}