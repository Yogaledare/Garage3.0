using Garage3._0.Data;
using Garage3._0.Models;
using Garage3._0.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding; // For ModelState
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // For View


namespace Garage3._0.Controllers;

public class MembersController : Controller {
    private readonly GarageDbContext _context;

    public MembersController(GarageDbContext context) {
        _context = context;
    }


    public async Task<IActionResult> Index() {

        var members = await _context.Members
            .Include(m => m.VehicleList)
            .ToListAsync();

        var memberViewModels = members.Select(m => new MemberViewModel {
            FirstName = m.Firstname,
            Surname = m.Surname,
            SocialSecurityNr = m.SocialSecurityNr.ToString(),
            Vehicles = m.VehicleList.ToList()
        });
        
        return View(memberViewModels);
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
            // var member = new Member
            // {
            //     SocialSecurityNr = model.SocialSecurityNr,
            //     Firstname = model.Firstname,
            //     Surname = model.Surname,
            //     VehicleList = new List<Vehicle>() // Initialize empty or handle vehicle addition
            // };

            // _context.Members.Add(member);
            // _context.SaveChanges();
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

            Console.WriteLine("hello!");
            return RedirectToAction("CreateVehicle", "Members", new { memberId = model.MemberId });
        }

        return View(model); 
    }
    
    
    

    // GET: Members/5/RegisterVehicle
    [HttpGet("Members/{memberId:int}/CreateVehicle")]
    public IActionResult CreateVehicle(int memberId) {
        var vehicleTypes = _context.VehicleTypes
            .Select(vt => new SelectListItem {
                Text = vt.VehicleTypeName,
                Value = vt.VehicleTypeId.ToString()
            })
            .ToList(); 

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
            return RedirectToAction(nameof(Index));
        }

        Console.WriteLine("inside create vehicle model state invalid");
        return View(input);
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