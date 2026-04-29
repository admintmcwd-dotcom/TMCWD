using Microsoft.AspNetCore.Mvc;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestDetailController : Controller
    {

        [HttpPost("SaveUpdate")]
        public ActionResult<bool> SaveUpdate([FromBody] RequestDetail detail)
        {

            try
            {
                using(var dbContext = new UserDbContext())
                {
                    if (detail.Id > 0) dbContext.RequestDetails.Update(detail);
                    else dbContext.RequestDetails.Add(detail);

                    int res = dbContext.SaveChanges();

                    if (res > 0) return Ok(true);
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(false);
        }

        [HttpPost("SaveMultiple")]
        public ActionResult<bool> SaveMultiple([FromBody] List<RequestDetail> details)
        {

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    if (details.Count <= 0) return BadRequest("No details to add to this request");

                    foreach(var item in details)
                    {
                        dbContext.RequestDetails.Add(item);
                    }

                    var res = dbContext.SaveChanges();
                    if (res > 0) return Ok(true);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(false);
        }

        [HttpGet("GetDetailsByRequestId")]
        public ActionResult<IEnumerable<RequestDetail>> GetDetailsByRequestId(int requestId)
        {
            IEnumerable<RequestDetail> details = new List<RequestDetail>();

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var data = dbContext.RequestDetails.Where(x => x.RequestId == requestId);
                    if (!data.Any()) return NotFound($"Details for request with id {requestId} not found");
                    details = data;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(details);
        }

    }
}
