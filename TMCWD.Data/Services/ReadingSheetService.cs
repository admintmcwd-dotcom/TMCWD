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

        [HttpGet("Get/{id}")]
        public async Task<ReadingSheet> Get(int id)
        {
            var readingSheet = await _context.ReadingSheets.Where(x => x.Id == id).FirstOrDefaultAsync();
            return readingSheet;
        }

        [HttpGet("GetAll")]
        public async Task<List<ReadingSheet>> GetAll()
        {
            var readingSheets = _context.ReadingSheets;
            return await readingSheets.ToListAsync();
        }

        [HttpGet("GetByAssignedTo/{assignedTo}")]
        public async Task<List<ReadingSheet>> GetByAssignedTo(int assignedTo)
        {
            var readingSheets = _context.ReadingSheets.Where(x => x.AssignedTo == assignedTo);
            return await readingSheets.ToListAsync();
        }

        [HttpGet("GetByZoneAndBook/{zone}/{book}")]
        public async Task<List<ReadingSheet>> GetByZoneAndBook(int zone, int book)
        {
            var readingSheets = _context.ReadingSheets.Where(x => x.Zone == zone && x.Book == book);
            return await readingSheets.ToListAsync();
        }

        [HttpGet("GetByZoneBookAndAssignedTo/{zone}/{book}/{assignedTo}")]
        public async Task<List<ReadingSheet>> GetByZoneBookAndAssignedTo(int zone, int book, int assignedTo)
        {
            var readingSheets = _context.ReadingSheets.Where(x => x.Zone == zone && x.Book == book && x.AssignedTo == assignedTo);
            return await readingSheets.ToListAsync();
        }

        [HttpGet("GetByBillingDate/{zone}/{book}/{dueDate}")]
        public async Task<ReadingSheet> GetByBillingDate(int zone, int book, DateTime billingDate)
        {
            var readingSheet = await _context.ReadingSheets.Where(x => x.Zone == zone && x.Book == book && x.BillingDate == billingDate).FirstOrDefaultAsync();
            return readingSheet;
        }

        [HttpGet("SaveUpdate/{userId}")]
        public async Task<ReadingSheet> SaveUpdate(int userId, ReadingSheet readingSheet)
        {
            if (readingSheet.Id > 0)
            {
                var forUpdate = await _context.ReadingSheets.Where(x => x.Zone == readingSheet.Zone && x.Book == readingSheet.Book && x.BillingDate == readingSheet.BillingDate).FirstOrDefaultAsync();
                if (forUpdate != null)
                {
                    forUpdate.Name = readingSheet.Name;
                    forUpdate.BillingDate = readingSheet.BillingDate;
                    forUpdate.Zone = readingSheet.Zone;
                    forUpdate.Book = readingSheet.Book;
                    forUpdate.AssignedTo = readingSheet.AssignedTo;
                    forUpdate.UpdatedBy = userId;
                    forUpdate.DateUpdated = DateTime.Now;
                    readingSheet = forUpdate;
                    _context.ReadingSheets.Update(readingSheet);
                }
            }
            else
            {
                readingSheet.CreatedBy = userId;
                readingSheet.DateCreated = DateTime.Now;
                _context.ReadingSheets.Add(readingSheet);
            }

            await  _context.SaveChangesAsync();

            return readingSheet;
        }

    }
}
