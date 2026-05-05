using Microsoft.AspNetCore.Mvc;

namespace TMCWD.Application.Controllers
{
    public class CustomerSupportController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
