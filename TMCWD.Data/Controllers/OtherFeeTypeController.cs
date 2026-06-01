using Microsoft.AspNetCore.Mvc;
using TMCWD.Data.Entities;
using TMCWD.Data.Services;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OtherFeeTypeController : Controller
    {

        private readonly IOtherFeeTypeService _service;

        public OtherFeeTypeController(IOtherFeeTypeService service)
        {
            _service = service;
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var otherFeeType = await _service.Get(id);
            if (otherFeeType == null) return NotFound();
            return Ok(otherFeeType);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var otherFeeTypes = await _service.GetAll();
            if(otherFeeTypes == null || !otherFeeTypes.Any()) return NotFound();
            return Ok(otherFeeTypes);
        }

        [HttpGet("GetByName/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var otherFeeTypes = await _service.GetByName(name);
            if (otherFeeTypes == null || !otherFeeTypes.Any()) return NotFound();
            return Ok(otherFeeTypes);
        }

        [HttpPost("SaveUpdate/{userId}")]
        public async Task<IActionResult> SaveUpdate(int userId, [FromBody] OtherFeeType otherFeeType)
        {
            var updatedOtherFeeType = await _service.SaveUpdate(userId, otherFeeType);
            if(updatedOtherFeeType == null) return NoContent();
            return Ok(updatedOtherFeeType);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var otherFeeType = await _service.Get(id);
            if(otherFeeType == null) return NotFound();
            var ret = await _service.Delete(otherFeeType);
            return Ok(ret);
        }

    }
}
