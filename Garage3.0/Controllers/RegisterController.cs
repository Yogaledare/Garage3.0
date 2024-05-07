using Garage3._0.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;


namespace Garage3._0.Controllers
{
	public class RegisterController : Controller
	{
		private readonly IGarageManager _manager;
        public RegisterController(IGarageManager manager)
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
			return RedirectToAction("Index",_manager);
		}

		// Handle vehicle check out
		[HttpGet]
		public ActionResult Logout(int? id)
		{
			if(id == null)
			{
                return RedirectToAction("Index");
            }			
			
            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Logout")]
        public ActionResult DeleteConfirmed(int id)
		{
			var parkingEvent = _manager.UnParkVehicle(id);

            return RedirectToAction("Index");
		}

    }
}
