using Microsoft.AspNetCore.Mvc;
using TMCWD.Data.Entities;
using TMCWD.Data.Services;

namespace TMCWD.Data.Controllers
{
    public class ReadingController : Controller
    {

        #region fields

        private readonly IReadingService _service;

        #endregion

        #region constructors

        public ReadingController(IReadingService service)
        {
            _service = service;
        }

        #endregion

        #region methods

        public async Task<IActionResult> Get(int id)
        {
            var reading = await _service.Get(id);
            if (reading == null) return NotFound();
            return Ok(reading);
        }

        public async Task<IActionResult> GetByAccount(int accountId)
        {
            var readings = await _service.GetByAccount(accountId);
            if (readings == null || !readings.Any()) return NotFound();
            return Ok(readings);
        }

        public async Task<IActionResult> GetByAccountAndBillingPeriod(int accountId, DateTime billingPeriod)
        {
            var reading = await _service.GetByAccountAndBillingPeriod(accountId, billingPeriod);
            if (reading == null) return NotFound();
            return Ok(reading);
        }

        public async Task<IActionResult> GetByReader(int readerId)
        {
            var readings = await _service.GetByReader(readerId);
            if (readings == null) return NotFound();
            return Ok(readings);
        }

        public async Task<IActionResult> GetByZoneAndBook(int zone, int book)
        {
            var readings = await _service.GetByZoneAndBook(zone, book);
            if (readings == null || !readings.Any()) return NotFound();
            return Ok(readings);
        }

        public async Task<IActionResult> SaveUpdate(int userId, Reading reading)
        {
            var savedReading = await _service.SaveUpdate(userId, reading);
            return Ok(savedReading);
        }

        #endregion

    }
}
