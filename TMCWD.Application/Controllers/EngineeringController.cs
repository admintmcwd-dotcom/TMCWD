using Microsoft.AspNetCore.Mvc;

namespace TMCWD.Application.Controllers
{
    public class EngineeringController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
