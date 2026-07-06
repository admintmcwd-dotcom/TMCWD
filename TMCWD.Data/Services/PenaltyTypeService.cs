using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;

namespace TMCWD.Data.Services
{
    public class PenaltyTypeService : IPenaltyTypeService
    {

        private readonly UserDbContext _context;

        public PenaltyTypeService(UserDbContext context)
        {
            _context = context;
        }

        public async Task<PenaltyType> Get(int id)
        {
            var penaltyType = await _context.PenaltyTypes.Where(pt => pt.Id == id).FirstOrDefaultAsync();
            return penaltyType;
        }

        public async Task<List<PenaltyType>> GetAll()
        {
            var penaltyTypes = await _context.PenaltyTypes.ToListAsync();
            return penaltyTypes;
        }

        public async Task<PenaltyType> SaveUpdate(int userId, PenaltyType penaltyType)
        {
            if(penaltyType.Id == 0)
            {
                penaltyType.CreatedBy = userId;
                penaltyType.DateCreated = DateTime.UtcNow;
                _context.PenaltyTypes.Add(penaltyType);
            }
            else
            {
                var existingPenaltyType = await _context.PenaltyTypes.Where(x => x.Id == penaltyType.Id).FirstOrDefaultAsync();
                if (existingPenaltyType != null)
                {
                    existingPenaltyType.Name = penaltyType.Name;
                    existingPenaltyType.UpdatedBy = userId;
                    existingPenaltyType.DateUpdated = DateTime.Now;
                    _context.PenaltyTypes.Update(existingPenaltyType);
                }
            }

            await _context.SaveChangesAsync();
            return penaltyType;
        }
    }
}
