using Microsoft.AspNetCore.Mvc;
using TMCWD.Data.Entities;
using TMCWD.Data.Services;

namespace TMCWD.Data.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PenaltyTypeController : Controller
    {
        private readonly IPenaltyTypeService _penaltyTypeService;

        public PenaltyTypeController(IPenaltyTypeService penaltyTypeService)
        {
            _penaltyTypeService = penaltyTypeService;
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var penaltyType = await _penaltyTypeService.Get(id);
            return Ok(penaltyType);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var penaltyTypes = await _penaltyTypeService.GetAll();
            return Ok(penaltyTypes);
        }

        [HttpGet("SaveUpdate/{userId}")]
        public async Task<IActionResult> SaveUpdate(int userId, PenaltyType penaltyType)
        {
            var result = await _penaltyTypeService.SaveUpdate(userId, penaltyType);
            return Ok(result);
        }

    }
}
