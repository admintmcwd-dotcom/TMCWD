using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InspectionTypeController : Controller
    {

        private readonly UserDbContext _dbContext;

        public InspectionTypeController(UserDbContext context)
        {
            this._dbContext = context;
        }

        [HttpGet("GetTypes")]
        public ActionResult<IEnumerable<InspectionType>> GetTypes()
        {
            IEnumerable<InspectionType> types = new List<InspectionType>();

            types = _dbContext.InspectionTypes;
            if (types == null || !types.Any()) return NotFound("No incident type found");

            return Ok(types);
        }

        [HttpGet("GetById")]
        public ActionResult<InspectionType> Get(int id)
        {
            InspectionType type = new();

            var res = _dbContext.InspectionTypes.Where(x => x.Id == id).FirstOrDefault();
            if (res == null) return NotFound($"Incident type id {id} not found");
            type = res;

            return Ok(type);
        }

        [HttpPost("SaveUpdate")]
        public ActionResult<bool> SaveUpdate([FromBody] InspectionType type)
        {
            if (type.Id == 0)
            {
                _dbContext.InspectionTypes.Add(type);
            }
            else
            {
                _dbContext.InspectionTypes.Update(type);
            }
            int res = _dbContext.SaveChanges();
            if (res > 0) return Ok(true);

            return Ok(false);
        }

    }
}
