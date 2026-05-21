using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System.Text.Json;
using TMCWD.Data.Entities;
using TMCWD.Data.Services;

namespace TMCWD.Data.Controllers
{

    [ApiController]
    [Route("api/{requestId}/[controller]")]
    public class MaterialController : Controller
    {
        private readonly IMaterialService _service;

        public MaterialController(IMaterialService service)
        {
            _service = service;
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var material = await _service.Get(id);

            if (material == null) return NotFound();

            return Ok(material);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var materials = await _service.GetAll();
            if (materials == null || !materials.Any()) return NotFound();
            return Ok(materials);
        }

        [HttpGet("GetByRequestId")]
        public async Task<IActionResult> GetByRequestId(int requestId)
        {
            var materials = await _service.GetByRequestId(requestId);
            if(materials == null || !materials.Any()) return NotFound();
            return Ok(materials);
        }

        [HttpPost("SaveUpdate/{userId}")]
        public async Task<IActionResult> SaveUpdate(int userId, int requestId, [FromBody] Material material)
        {
            var updatedMaterial = await _service.SaveUpdate(userId, requestId, material);
            if (material == null) return NoContent();
            return Ok(updatedMaterial);
        }

        [HttpPut("UpdateQuantityOrNewUnitCost/{userId}")]
        public async Task<IActionResult> UpdateQuantityOrNewUnitCost(int userId, int requestId, [FromBody]Material updateMaterial)
        {
            var material = _service.UpdateQuantityOrNewUnitCost(userId, requestId, updateMaterial);
            if(material == null) return NotFound();
            return Ok(material);
        }

    }

}
