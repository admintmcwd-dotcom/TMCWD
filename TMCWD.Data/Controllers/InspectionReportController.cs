using Microsoft.AspNetCore.Mvc;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InspectionReportController : Controller
    {

        [HttpPost("SaveUpdate")]
        public ActionResult<bool> SaveUpdate([FromBody] InspectionReport report)
        {
            try
            {
                using(var dbContext = new UserDbContext())
                {
                    if (report.Id > 0) dbContext.InspectionReports.Update(report);
                    else dbContext.InspectionReports.Add(report);
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
        public ActionResult<InspectionReport> GetById(int id)
        {
            InspectionReport report = new();
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var data = dbContext.InspectionReports.FirstOrDefault(x => x.Id == id);
                    if (data == null) return NotFound($"Inspection report with id {id} is not found.");
                    report = data;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(report);
        }

        [HttpGet("GetByRequestId")]
        public ActionResult<IEnumerable<InspectionReport>> GetByRequestId(int requestId)
        {
            IEnumerable<InspectionReport> reports = new List<InspectionReport>();
            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var data = dbContext.InspectionReports.Where(x => x.RequestId == requestId);
                    if (data == null || !data.Any()) return NotFound($"Inspection report(s) with request id {requestId} is not found.");
                    reports = data;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(reports);
        }

    }
}
