using Microsoft.AspNetCore.Mvc;
using TMCWD.Model.Engineering;
using TMCWD.Model.Administrator;
using System.Text.Json;
using TMCWD.Engineering;
using TMCWD.Application.Models;

namespace TMCWD.Application.Controllers
{
    public class EngineeringController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Inventory()
        {
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            User currentUser = new();

            InventoryViewModel model = new();
            
            if (!String.IsNullOrEmpty(jsonCurrentUser.Trim()))
            {
                currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
                model.CurrentUser = currentUser;
                ViewBag.Role = currentUser.Role;
            }

            InventoryTransaction invTrans = new();
            model.Inventory = invTrans.GetAll();

            return View(model);
        }

    }
}
