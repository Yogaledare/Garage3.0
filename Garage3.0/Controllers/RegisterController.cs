using Garage3._0.Models;
using Microsoft.AspNetCore.Mvc;

namespace Garage3._0.Controllers
{
	public class RegisterController : Controller
	{
		private readonly GarageManager _manager;
        public RegisterController(GarageManager manager)
        {
            _manager = manager;
        }
		
        public IActionResult Index()
		{
			return View(_manager);
		}
		// Handle vehicle check in 
		[HttpPost]
		public ActionResult Login(int id)
		{
			var parkEvent = _manager.ParkVehicle(id);
			return RedirectToAction("Index");
		}

		// Handle vehicle check out
		[HttpPost]
		public ActionResult Logout(int id)
		{
            Console.WriteLine(id);
            return RedirectToAction("Index");
		}
	}
}
