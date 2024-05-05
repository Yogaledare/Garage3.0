using Microsoft.AspNetCore.Mvc;

namespace Garage3._0.Controllers
{
	public class RegisterController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		// Handle vehicle check in 
		[HttpPost]
		public ActionResult Login(int id)
		{
			Console.WriteLine(id);
			return RedirectToAction("Index", "Home");
		}

		// Handle vehicle check out
		[HttpPost]
		public ActionResult Logout(int id)
		{
            Console.WriteLine(id);
            return RedirectToAction("Index", "Home");
		}
	}
}
