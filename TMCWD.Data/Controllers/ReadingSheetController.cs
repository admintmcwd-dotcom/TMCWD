using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using TMCWD.Data.Entities;
using TMCWD.Data.Services;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReadingSheetController : Controller
    {

        private readonly IReadingSheetService _readingSheetService;

        public ReadingSheetController(IReadingSheetService readingSheetService)
        {
            _readingSheetService = readingSheetService;
        }

        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var readingSheet = await _readingSheetService.Get(id);
            if(readingSheet == null) return NotFound();
            return Ok(readingSheet);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var readingSheets = await _readingSheetService.GetAll();
            if (readingSheets == null || !readingSheets.Any()) return NotFound();
            return Ok(readingSheets);
        }

        [HttpGet("GetByAssignedTo/{assignedTo}")]
        public async Task<IActionResult> GetByAssignedTo(int assignedTo)
        {
            var readingSheets = await _readingSheetService.GetByAssignedTo(assignedTo);
            if (readingSheets == null || !readingSheets.Any()) return NotFound();
            return Ok(readingSheets);
        }

        [HttpGet("GetByZoneAndBook/{zone}/{book}")]
        public async Task<IActionResult> GetByZoneAndBook(int zone, int book)
        {
            var readingSheets = await _readingSheetService.GetByZoneAndBook(zone, book);
            if (readingSheets == null || !readingSheets.Any()) return NotFound();
            return Ok(readingSheets);
        }

        [HttpGet("GetByZoneBookAndAssignedTo/{zone}/{book}/{assignedTo}")]
        public async Task<IActionResult> GetByZoneBookAndAssignedTo(int zone, int book, int assignedTo)
        {
            var readingSheets = await _readingSheetService.GetByZoneBookAndAssignedTo(zone, book, assignedTo);
            if (readingSheets == null || !readingSheets.Any()) return NotFound();
            return Ok(readingSheets);
        }

        public async Task<IActionResult> GetByBillingDate(int zone, int book, DateTime dueDate)
        {
            var readingSheet = await _readingSheetService.GetByBilligDate(zone, book, dueDate);
            if (readingSheet == null) return NotFound();
            return Ok(readingSheet);
        }

        [HttpGet("SaveUpdate/{userId}")]
        public async Task<IActionResult> SaveUpdate(int userId, ReadingSheet readingSheet)
        {
            var readingSheetCheck = await _readingSheetService.GetByBilligDate(readingSheet.Zone, readingSheet.Book, readingSheet.BillingDate);
            if (readingSheetCheck != null) return Ok(readingSheetCheck);
            var result = await _readingSheetService.SaveUpdate(userId, readingSheet);
            return Ok(result);
        }

    }
}
