using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : Controller
    {

        private readonly UserDbContext _dbContext;

        public RequestController(UserDbContext context)
        {
            _dbContext = context;
        }

        [HttpPost("SaveUpdate")]
        public ActionResult<int> SaveUpdate([FromBody] Request request)
        {
            if (request.Id > 0) _dbContext.Requests.Update(request);
            else
            {
                var lastRequest = _dbContext.Requests.Where(x => x.DateCreated.Year == DateTime.Now.Year && x.DateCreated.Month == DateTime.Now.Month).OrderByDescending(x => x.ControlNumber).FirstOrDefault();
                if(lastRequest != null)
                {
                    int.TryParse(lastRequest.ControlNumber.Split('-')[^1].Trim(), out int lastCount);
                    if (lastCount > 0)
                    {
                        lastCount = lastCount + 1;
                        request.ControlNumber = $"TKT{request.DateCreated.ToString("yyyy")}-{request.DateCreated.ToString("MM")}-{(lastCount).ToString().PadLeft(4, '0')}";
                    }
                }

                if(String.IsNullOrEmpty(request.ControlNumber.Trim()))
                    request.ControlNumber = $"TKT{request.DateCreated.ToString("yyyy")}-{request.DateCreated.ToString("MM")}-{10.ToString().PadLeft(4, '0')}";

                _dbContext.Requests.Add(request);
            }

            int res = _dbContext.SaveChanges();
            if (res > 0) return Ok(request.Id);

            return Ok(0);
        }

        [HttpGet("GetById")]
        public ActionResult<Request> GetById(int id)
        { 
            var data = _dbContext.Requests.Where(x => x.Id == id).FirstOrDefault();
            if (data == null) return NotFound($"Request with id {id} not found");
            return Ok(data);
        }

        [HttpGet("GetRequests")]
        public ActionResult<IEnumerable<Request>> GetRequests()
        {
            var data = _dbContext.Requests;
            if (!data.Any()) return NotFound("No request(s) found");
            return Ok(data);
        }

        [HttpGet("GetByUserId")]
        public ActionResult<IEnumerable<Request>> GetByUserId(int userId)
        {
            var data = _dbContext.Requests.Where(x => x.CreatedBy == userId);
            if (!data.Any()) return NotFound($"No request(s) found for user with id {userId}");
            return Ok(data);
        }

        [HttpGet("GetByCustomerId")]
        public ActionResult<IEnumerable<Request>> GetByCustomerId(int customerId)
        {
            var data = _dbContext.Requests.Where(x => x.CustomerId == customerId);
            if (!data.Any()) return NotFound($"No request(s) found for customer with id {customerId}");
            return Ok(data);
        }

    }
}
