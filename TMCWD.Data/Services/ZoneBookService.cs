using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;

namespace TMCWD.Data.Services
{
    public class ZoneBookService : IZoneBookService
    {

        #region fields

        private readonly UserDbContext _context;

        #endregion

        #region constructors

        public ZoneBookService(UserDbContext context)
        {
            _context = context;
        }

        #endregion

        #region methods

        public async Task<ZoneBook> Get(int id)
        {
            var zoneBook = await _context.ZoneBooks.Where(x => x.Id == id).FirstOrDefaultAsync();
            return zoneBook;
        }

        public async Task<List<ZoneBook>> GetAll()
        {
            var zoneBooks = await _context.ZoneBooks.ToListAsync();
            return zoneBooks;
        }

        public async Task<List<ZoneBook>> GetByWeek(int week)
        {
            var zoneBooks = await _context.ZoneBooks.Where(x => x.Week == week).ToListAsync();
            return zoneBooks;
        }

        public async Task<List<ZoneBook>> GetByZone(int zone)
        {
            var zoneBooks = await _context.ZoneBooks.Where(x => x.Zone == zone).ToListAsync();
            return zoneBooks;
        }

        public async Task<ZoneBook> GetByZoneAndBook(int zone, int book)
        {
            var zoneBook = await _context.ZoneBooks.Where(x => x.Zone == zone && x.Book == book).FirstOrDefaultAsync();
            return zoneBook;
        }

        #endregion

    }
}
