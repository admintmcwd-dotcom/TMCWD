using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestDetailController : Controller
    {

        private readonly UserDbContext _dbContext;

        public RequestDetailController(UserDbContext context)
        {
            _dbContext = context;
        }

        [HttpPost("SaveUpdate")]
        public ActionResult<bool> SaveUpdate([FromBody] RequestDetail detail)
        {

            if (detail.Id > 0) _dbContext.RequestDetails.Update(detail);
            else _dbContext.RequestDetails.Add(detail);

            int res = _dbContext.SaveChanges();

            if (res > 0) return Ok(true);

            return Ok(false);
        }

        [HttpPost("SaveMultiple")]
        public ActionResult<bool> SaveMultiple([FromBody] List<RequestDetail> details)
        {

            if (details.Count <= 0) return BadRequest("No details to add to this request");

            foreach (var item in details)
            {
                _dbContext.RequestDetails.Add(item);
            }

            var res = _dbContext.SaveChanges();
            if (res > 0) return Ok(true);

            return Ok(false);
        }

        [HttpGet("GetDetailsByRequestId")]
        public ActionResult<IEnumerable<RequestDetail>> GetDetailsByRequestId(int requestId)
        {
            var data = _dbContext.RequestDetails.Where(x => x.RequestId == requestId);
            if (!data.Any()) return NotFound($"Details for request with id {requestId} not found");

            return Ok(data);
        }

    }
}
