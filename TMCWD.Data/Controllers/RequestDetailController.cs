using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Data.Services;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/{requestId}/[controller]")]
    public class RequestDetailController : Controller
    {

        private readonly IRequestDetailService _requestDetailService;

        public RequestDetailController(IRequestDetailService service)
        {
            _requestDetailService = service;
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<RequestDetail>> Get(int id)
        {
            var requestDetail = await _requestDetailService.Get(id);
            if (requestDetail == null) return NotFound($"Request detail with id {id} was not found.");
            return Ok(requestDetail);
        }

        [HttpPost("SaveUpdate/{userId}")]
        public async Task<ActionResult<RequestDetail>> SaveUpdate(int userId, [FromRoute] int requestId, [FromBody] RequestDetail detail)
        {
            var updateDetail = await _requestDetailService.SaveUpdate(userId, requestId, detail);
            if (updateDetail == null) return BadRequest("Problem(s) encountered while saving request detail");
            return Ok(updateDetail);
        }

        [HttpPost("SaveMultiple/{userId}")]
        public async Task<ActionResult<IEnumerable<RequestDetail>>> SaveMultiple(int userId, [FromRoute] int requestId, [FromBody] List<RequestDetail> details)
        {

            if (details.Count <= 0) return BadRequest("No details to add to this request");

            var updatedDetails = await _requestDetailService.SaveMultiple(userId, requestId, details);

            var noId = updatedDetails.Where(x => x.Id == 0).FirstOrDefault();

            if (noId != null) return BadRequest("Not all request details was saved");

            return Ok(updatedDetails);
        }

        [HttpGet("GetByRequestId")]
        public async Task<ActionResult<IEnumerable<RequestDetail>>> GetByRequestId([FromRoute] int requestId)
        {
            var requestDetails = await _requestDetailService.GetByRequestId(requestId);

            if (requestDetails == null || !requestDetails.Any()) return NotFound($"No request detail(s) with request id {requestId} was found.");

            return Ok(requestDetails);
        }

    }
}
