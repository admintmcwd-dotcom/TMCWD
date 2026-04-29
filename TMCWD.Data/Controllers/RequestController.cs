using Microsoft.AspNetCore.Mvc;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RequestController : Controller
    {

        [HttpPost("SaveUpdate")]
        public ActionResult<bool> SaveUpdate([FromBody] Request request)
        {
            try
            {
                using(var dbContext = new UserDbContext())
                {
                    if (request.Id > 0) dbContext.Requests.Update(request);
                    else dbContext.Requests.Add(request);

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
        public ActionResult<Request> GetById(int id)
        {
            Request request = new();

            try
            {
                using(var dbContext = new UserDbContext())
                {
                    var data = dbContext.Requests.Where(x => x.Id == id).FirstOrDefault();
                    if (data == null) return NotFound($"Request with id {id} not found");
                    request = data;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(request);
        }

        [HttpGet("GetRequests")]
        public ActionResult<IEnumerable<Request>> GetRequests()
        {
            IEnumerable<Request> requests = new List<Request>();

            try
            {
                using(var dbContext = new UserDbContext())
                {
                    var data = dbContext.Requests;
                    if (!data.Any()) return NotFound("No request(s) found");
                    requests = data;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(requests);
        }

        [HttpGet("GetByUserId")]
        public ActionResult<IEnumerable<Request>> GetByUserId(int userId)
        {
            IEnumerable<Request> requests = new List<Request>();

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var data = dbContext.Requests.Where(x => x.UserId == userId);
                    if (!data.Any()) return NotFound($"No request(s) found for user with id {userId}");
                    requests = data;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(requests);
        }

        [HttpGet("GetByCustomerId")]
        public ActionResult<IEnumerable<Request>> GetByCustomerId(int customerId)
        {
            IEnumerable<Request> requests = new List<Request>();

            try
            {
                using(var dbContext = new UserDbContext())
                {
                    var data = dbContext.Requests.Where(x =>x.CustomerId == customerId);
                    if (!data.Any()) return NotFound($"No request(s) found for customer with id {customerId}");
                    requests = data;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(requests);
        }

    }
}
