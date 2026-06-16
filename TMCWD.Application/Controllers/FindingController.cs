using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TMCWD.CustomerSupport;
using TMCWD.Model.CustomerSupport;
using TMCWD.Services;

namespace TMCWD.Application.Controllers
{
    public class FindingController : Controller
    {

        private readonly FindingTransaction _findingTransaction;
        private readonly AuthenticatedUserService _authenticatedUser;

        public FindingController(FindingTransaction findingTransaction, AuthenticatedUserService authenticatedUser)
        {
            _findingTransaction = findingTransaction;
            _authenticatedUser = authenticatedUser;
        }

        [HttpPost]
        //public async Task<IActionResult> SaveFinding(int jobOrderId, Finding finding, [FromBody] List<IFormFile>  inspectionFiles)
        public async Task<IActionResult> SaveFinding(int jobOrderId, List<IFormFile> data)
        {
            if (data == null) return BadRequest();

            var content = JsonSerializer.Serialize(data);

            using var doc = JsonDocument.Parse(content);

            var root = doc.RootElement;

            var findingProp = root.GetProperty("finding");
            var fileProp = root.GetProperty("inspectionFiles");
            if(findingProp.ValueKind == JsonValueKind.Null || fileProp.ValueKind == JsonValueKind.Null) return BadRequest();

            var serializerOption = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };

            var finding = findingProp.Deserialize<Finding>(serializerOption);
            var files = fileProp.Deserialize<List<IFormFile>>(serializerOption);

            if (finding == null || files == null) return BadRequest();

            var updatedFinding = await _findingTransaction.SaveUpdate(_authenticatedUser.User.Id, jobOrderId, finding);
            return View();
        }

    }
}
