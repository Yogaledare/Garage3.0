using Garage3._0.Data;
using Garage3._0.Models;
using Garage3._0.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding; // For ModelState
using Microsoft.AspNetCore.Mvc.Rendering; // For View


namespace Garage3._0.Controllers;

public class MembersController : Controller {
    
    
    private readonly GarageDbContext _context;

    public MembersController(GarageDbContext context)
    {
        _context = context;
    }


    public IActionResult Index() {
        return View(); 
    }

    // GET: Members/Create
    public IActionResult Create() {
        return View();
    }

    // POST: Members/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateMemberViewModel model)
    {
        if (ModelState.IsValid)
        {
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
        return View(model);
    }
    
    
}