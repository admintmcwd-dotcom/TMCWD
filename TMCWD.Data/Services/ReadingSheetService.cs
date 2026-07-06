using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;

namespace TMCWD.Data.Services
{
    public class ReadingSheetService : IReadingSheetService
    {

        private readonly UserDbContext _context;

        public ReadingSheetService(UserDbContext context)
        {
            _context = context;
        }

        public async Task<ReadingSheet> Get(int id)
        {
            var readingSheet = await _context.ReadingSheets.Where(x => x.Id == id).FirstOrDefaultAsync();
            return readingSheet;
        }

        public async Task<List<ReadingSheet>> GetAll()
        {
            var readingSheets = _context.ReadingSheets;
            return await readingSheets.ToListAsync();
        }

        public async Task<List<ReadingSheet>> GetByAssignedTo(int assignedTo)
        {
            var readingSheets = _context.ReadingSheets.Where(x => x.AssignedTo == assignedTo);
            return await readingSheets.ToListAsync();
        }

        public async Task<List<ReadingSheet>> GetByZoneAndBook(int zone, int book)
        {
            var readingSheets = _context.ReadingSheets.Where(x => x.Zone == zone && x.Book == book);
            return await readingSheets.ToListAsync();
        }

        public async Task<List<ReadingSheet>> GetByZoneBookAndAssignedTo(int zone, int book, int assignedTo)
        {
            var readingSheets = _context.ReadingSheets.Where(x => x.Zone == zone && x.Book == book && x.AssignedTo == assignedTo);
            return await readingSheets.ToListAsync();
        }

        [HttpGet("GetByBillingDate/{zone}/{book}/{dueDate}")]
        public async Task<ReadingSheet> GetByBillingDate(int zone, int book, DateTime dueDate)
        {
            var readingSheet = await _context.ReadingSheets.Where(x => x.Zone == zone && x.Book == book && x.BillingDate == dueDate).FirstOrDefaultAsync();
            return readingSheet;
        }

        [HttpGet("SaveUpdate/{userId}")]
        public async Task<ReadingSheet> SaveUpdate(int userId, ReadingSheet readingSheet)
        {
            if (readingSheet.Id > 0)
            {
                readingSheet.UpdatedBy = userId;
                readingSheet.DateUpdated = DateTime.Now;
            }
            else
            {
                readingSheet.CreatedBy = userId;
                readingSheet.DateCreated = DateTime.Now;
            }
            return readingSheet;
        }

    }
}
