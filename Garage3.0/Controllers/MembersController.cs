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
    private readonly IMemberService _memberService;
    private readonly IVehicleService _vehicleService;


    public MembersController(IMemberService memberService, IVehicleService vehicleService) {
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
    public IActionResult CreateMember(CreateMemberViewModel input) {
        if (ModelState.IsValid) {
            var member = new Member {
                SocialSecurityNr = input.SocialSecurityNr,
                Firstname = input.Firstname,
                Surname = input.Surname,
            };

            _memberService.RegisterMember(member);
            return RedirectToAction(nameof(Index));
        }

        return View(input);
    }


    public IActionResult CreateVehicleType(int memberId) {
        var model = new CreateVehicleTypeViewModel {
            MemberId = memberId
        };

        return View(model);
    }


    [HttpPost]
    public IActionResult CreateVehicleType(CreateVehicleTypeViewModel model) {
        if (ModelState.IsValid) {
            _vehicleService.CreateVehicleType(model);
            return RedirectToAction("CreateVehicle", "Members", new {memberId = model.MemberId});
        }

        return View(model);
    }


    // GET: Members/5/RegisterVehicle
    [HttpGet("Members/{memberId:int}/CreateVehicle")]
    public IActionResult CreateVehicle(int memberId) {
        Console.WriteLine("hello mum!");
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
            _vehicleService.CreateVehicle(input);
            return RedirectToAction(nameof(Index));
        }

        input.VehicleTypeOptions = _vehicleService.GetVehicleTypeOptions();
        Console.WriteLine("inside create vehicle model state invalid");
        return View(input);
    }
}


// var existingMember = _context.Members.FirstOrDefault(x => x.SocialSecurityNr == input.SocialSecurityNr);
// if (existingMember != null)
// {
//     ModelState.AddModelError(nameof(CreateMemberViewModel.SocialSecurityNr), "Social Security Number already exists.");
//     return View(input);
// }

// var fullName = input.Firstname + " " + input.Surname;
// var memberWithFullName = _context.Members.FirstOrDefault(x => (x.Firstname + " " + x.Surname) == fullName);
// if (memberWithFullName != null)
// {
//     ModelState.AddModelError("", "A user with the same full name already exists.");
//     return View(input);
// }