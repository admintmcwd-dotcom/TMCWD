using Microsoft.AspNetCore.Mvc;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InspectionTypeController : Controller
    {
        [HttpGet("GetTypes")]
        public ActionResult<IEnumerable<InspectionType>> GetTypes()
        {
            IEnumerable<InspectionType> types = new List<InspectionType>();

            try
            {
                using(var dbContext = new UserDbContext())
                {
                    types = dbContext.InspectionTypes;
                    if (!types.Any()) return NotFound("No incident type found");
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(types);
        }

        [HttpGet("GetById")]
        public ActionResult<InspectionType> Get(int id)
        {
            InspectionType type = new();

            try
            {
                using(var dbContext = new UserDbContext())
                {
                    var res = dbContext.InspectionTypes.Where(x => x.Id == id).FirstOrDefault();
                    if (res == null) return NotFound($"Incident type id {id} not found");

                    type.Id = res.Id;
                    type.Name = res.Name;
                    type.DateCreated = res.DateCreated;
                    type.DateUpdated = res.DateUpdated;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(type);
        }

        [HttpPost("SaveUpdate")]
        public ActionResult<bool> SaveUpdate([FromBody] InspectionType type)
        {
            try
            {
                using(var dbContext = new UserDbContext())
                {
                    if (type.Id == 0)
                    {
                        dbContext.InspectionTypes.Add(type);
                    }
                    else
                    {
                        dbContext.InspectionTypes.Update(type);
                    }
                    int res = dbContext.SaveChanges();
                    if(res > 0) return Ok(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(false);
        }
    }
}
