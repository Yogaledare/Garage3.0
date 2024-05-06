using Garage3._0.Data;
using Garage3._0.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Garage3._0.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly GarageDbContext _context;
        private readonly GarageManager _manager;

        public HomeController(ILogger<HomeController> logger, GarageDbContext context,GarageManager manager)
        {
            _logger = logger;
            _context = context;
            _manager = manager;
        }

        public IActionResult Index() {
            // var v = new Vehicle();

            // var e = v.VehicleType.VehicleTypeName;

            ////Parking logic test
            //Vehicle vehicle = new Vehicle();
            //vehicle.VehicleType = new VehicleType();
            //vehicle.VehicleType.ParkingSpaceRequirement = 2;
            //_manager.ParkVehicle(vehicle);

            //data seeding test
            //var generator = new FakeDataGenerator();
            //var member = generator.GenerateMember(10);
            //var m = member.First();
            //var v = m.VehicleList.First();
            //var vt = v.VehicleType;
            //var result =  _manager.ParkVehicle(v);
            //if(result != null)
            //{
            //    //show data
            //    return View(result);
            //}
            //data seeding!
            //var seedData = new SeedDataGenerator(_context);
            //seedData.Generate();
          
            var result = _context.Members.Include(m=>m.VehicleList);
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
