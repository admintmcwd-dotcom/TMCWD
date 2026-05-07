using Microsoft.AspNetCore.Mvc;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationController : Controller
    {

        [HttpPost("SaveUpdate")]
        public ActionResult<bool> SaveUpdate([FromBody] Recommendation recommendation)
        {
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    if (recommendation.Id > 0) dbContext.Recommendations.Update(recommendation);
                    else dbContext.Recommendations.Add(recommendation);
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

        [HttpGet("GetById")]
        public ActionResult<Recommendation> GetById(int id)
        {
            Recommendation recommendation = new();

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var data = dbContext.Recommendations.FirstOrDefault(x => x.Id == id);
                    if (data == null) return NotFound($"Recommendation with id {id} is not found");
                    recommendation = data;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(recommendation);
        }

        [HttpGet("GetByRequestId")]
        public ActionResult<IEnumerable<Recommendation>> GetByRequestId(int requestId)
        {
            IEnumerable<Recommendation> recommendations = new List<Recommendation>();

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var data = dbContext.Recommendations.Where(x => x.RequestId == requestId);
                    if (data == null || !data.Any()) return NotFound($"Recommendation(s) with request id {requestId} is not found");
                    recommendations = data;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(recommendations);
        }

    }
}
