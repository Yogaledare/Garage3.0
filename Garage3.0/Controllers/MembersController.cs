using Garage3._0.Data;
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

    public MembersController(GarageDbContext context, IMemberService memberService) {
        _context = context;
        _memberService = memberService;
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
    public IActionResult CreateMember(CreateMemberViewModel input) {
        if (ModelState.IsValid) {
            var existingMember = _context.Members.FirstOrDefault(x => x.SocialSecurityNr == input.SocialSecurityNr);
            if (existingMember != null)
            {
                ModelState.AddModelError(nameof(CreateMemberViewModel.SocialSecurityNr), "Social Security Number already exists.");
                return View(input);
            }

            var fullName = input.Firstname + " " + input.Surname;
            var memberWithFullName = _context.Members.FirstOrDefault(x => (x.Firstname + " " + x.Surname) == fullName);
            if (memberWithFullName != null)
            {
                ModelState.AddModelError("", "A user with the same full name already exists.");
                return View(input);
            }



            var member = new Member
            {
                SocialSecurityNr = input.SocialSecurityNr,
                Firstname = input.Firstname,
                Surname = input.Surname,
                //VehicleList = new List<Vehicle>() // Initialize empty or handle vehicle addition
            };

            _memberService.RegisterMember(member);
            return RedirectToAction(nameof(Index)); // Redirect to the index or another appropriate view
        }

        Console.WriteLine("inside create member model state invalid");
        return View(input);
    }


    // public IActionResult CreateVehicle() {
    //     return View();
    // }


    public IActionResult CreateVehicleType(int memberId) {

        var model = new CreateVehicleTypeViewModel {
            MemberId = memberId
        };
        
        return View(model); 
    }
    
    
    [HttpPost]
    public IActionResult CreateVehicleType(CreateVehicleTypeViewModel model) {

        if (ModelState.IsValid) {
            var vehicleType = new VehicleType {
                VehicleTypeName = model.VehicleTypeName,
                ParkingSpaceRequirement = model.ParkingSpaceRequirement
            };

            // Add wheel configurations
            foreach (var wheels in model.AllowedWheelNumbers) {
                vehicleType.WheelConfigurations.Add(new WheelConfiguration {
                    NumWheels = wheels
                });
            }

            // Save the new vehicle type and configurations
            _context.VehicleTypes.Add(vehicleType);
            _context.SaveChanges();

            return RedirectToAction("CreateVehicle", "Members", new { memberId = model.MemberId });
        }

        return View(model); 
    }
    
    
    

    // GET: Members/5/RegisterVehicle
    [HttpGet("Members/{memberId:int}/CreateVehicle")]
    public IActionResult CreateVehicle(int memberId) {
        Console.WriteLine("hello mum!");
        var vehicleTypes = GetVehicleTypeOptions();

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
        
        input.VehicleTypeOptions = GetVehicleTypeOptions();
        Console.WriteLine("inside create vehicle model state invalid");
        return View(input);
    }
    
    private List<SelectListItem> GetVehicleTypeOptions() {
        // This method retrieves the vehicle type options to be used in both the GET and POST methods
        return _context.VehicleTypes
            .Select(vt => new SelectListItem {
                Text = vt.VehicleTypeName,
                Value = vt.VehicleTypeId.ToString()
            })
            .ToList();
    }
    
}


// // POST: Members/5/RegisterVehicle
// [HttpPost("Members/{memberId:int}/CreateVehicle")]
// public IActionResult CreateVehicle(CreateVehicleViewModel model)
// {
//     if (ModelState.IsValid)
//     {
//         // Logic to save the vehicle to the database, linked to the member
//         return RedirectToAction("Index");
//     }
//     return View(model);
// }