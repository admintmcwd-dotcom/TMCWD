using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TMCWD.Administration;
using TMCWD.Model.Administrator;
using TMCWD.Application.Models;
using System.Text.Json;

namespace TMCWD.Application.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new LoginViewModel() { Email = string.Empty, Password = string.Empty });
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            ApplicationLoginTransaction login = new ApplicationLoginTransaction(email, password);
            User? currentUser = login.Login();
            if(currentUser != null)
            {
                var userJson = JsonSerializer.Serialize(currentUser);
                HttpContext.Session.SetString("currentUser", userJson);
                switch (currentUser.Role)
                {
                    case (int)UserRole.CustomerRepresentative:
                        return RedirectToAction("Index", "CustomerSupport");
                    case (int)UserRole.SuperAdmin:
                        return RedirectToAction("Index", "Admin");
                    case (int)UserRole.Engineer:
                        return RedirectToAction("Inventory", "Engineering");
                }
            }
            return View("Index", new LoginViewModel() { Email = string.Empty, Password = string.Empty });
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
