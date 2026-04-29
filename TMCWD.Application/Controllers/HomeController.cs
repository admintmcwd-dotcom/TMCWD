using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TMCWD.Administration;
using TMCWD.Application.Models;

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
            if(login.Login())
            {
                return RedirectToAction("Index", "Engineering");
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
