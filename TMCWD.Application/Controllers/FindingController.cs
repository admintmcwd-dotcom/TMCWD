using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TMCWD.Application.Models;
using TMCWD.CustomerSupport;
using TMCWD.Model.CustomerSupport;
using TMCWD.Services;

namespace TMCWD.Application.Controllers
{
    public class FindingController : Controller
    {

        private readonly FindingTransaction _findingTransaction;
        private readonly AuthenticatedUserService _authenticatedUser;
        private readonly RequestFileTransaction _requestFileTransaction;

        public FindingController(FindingTransaction findingTransaction, AuthenticatedUserService authenticatedUser, RequestFileTransaction requestFileTransaction)
        {
            _findingTransaction = findingTransaction;
            _authenticatedUser = authenticatedUser;
            _requestFileTransaction = requestFileTransaction;
        }


        [HttpPost]
        public async Task<IActionResult> SaveFinding(int jobOrderId, [FromBody] Finding finding)
        {
            if (finding == null || string.IsNullOrEmpty(finding.Detail.Trim())) return NoContent();

            var savedFinding = await _findingTransaction.SaveUpdate(_authenticatedUser.User.Id, jobOrderId, finding);
            if (savedFinding == null || savedFinding.Id <= 0) return NoContent();
            return Ok(savedFinding);
        }

        [HttpPost]
        public async Task<IActionResult> SaveFindingFile(int jobOrderId, List<IFormFile> files)
        {
            if (files == null || !files.Any()) return Ok(false);

            var destinationPath = Path.GetFullPath($"../Files/{DateTime.Now.ToString("yyyyMMdd")}");
            if (!Directory.Exists(destinationPath))
            {
                Directory.CreateDirectory(destinationPath);
            }

            List<RequestFile> requestFiles = new();

            foreach (var file in files)
            {
                RequestFile reqFile = new()
                {
                    JobOrderId = 0,
                    OriginalFilename = file.FileName,
                    PhysicalFilename = $"{Guid.NewGuid().ToString().Replace("-", "")}.{Path.GetExtension(file.FileName)}",
                    RequestType = RequestFileType.Finding,
                    Path = destinationPath,
                    CreatedBy = _authenticatedUser.User.Id
                };

                requestFiles.Add(reqFile);

                using (Stream stream = new FileStream(Path.Combine(destinationPath, reqFile.PhysicalFilename), FileMode.Create, FileAccess.Write))
                {
                    await file.CopyToAsync(stream);
                }

            }
            List<RequestFile> savedFiles = await _requestFileTransaction.SaveRange(requestFiles);

            return Ok(savedFiles != null && !savedFiles.Any());
        }

    }
}
