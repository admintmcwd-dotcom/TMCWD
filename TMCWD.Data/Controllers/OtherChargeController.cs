using Microsoft.AspNetCore.Mvc;
using TMCWD.Data.Services;

namespace TMCWD.Data.Controllers
{
    public class OtherChargeController : Controller
    {

        #region fields

        private readonly IOtherChargeService _service;

        #endregion

        #region constructors

        public OtherChargeController(IOtherChargeService service)
        {
            _service = service;
        }

        #endregion

        #region methods

        public async Task<IActionResult> Get(int id)
        {
            var otherCharges = await _service.Get(id);
            if (otherCharges == null) return NotFound();
            return Ok(otherCharges);
        }

        #endregion

    }
}
