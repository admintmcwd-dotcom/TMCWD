using Microsoft.AspNetCore.Mvc;

namespace TMCWD.Application.Controllers
{
    public class BillingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> GetById(int id)
        {

            return Ok();
        }
    }
}
