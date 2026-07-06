using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;

namespace TMCWD.Data.Services
{
    public class PenaltyService
    {

        private readonly UserDbContext _context;

        public PenaltyService(UserDbContext context)
        {
            _context = context;
        }

        public async Task<Penalty> Get(int id)
        {
            return await _context.Penalties.Where(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<Penalty>> GetAll()
        {
            return await _context.Penalties.ToListAsync();
        }

        public async Task<List<Penalty>> GetByBillingReferenceId(string billingReferenceId)
        {
            return await _context.Penalties.Where(x => x.BillingReferenceId == billingReferenceId).ToListAsync();
        }

        public async Task<Penalty> SaveUpdate(int userId, Penalty penalty)
        {
            if (penalty.Id == 0)
            {
                penalty.CreatedBy = userId;
                penalty.DateCreated = DateTime.Now;
                _context.Penalties.Add(penalty);
            }
            else
            {
                var existingPenalty = await _context.Penalties.Where(x => x.Id == penalty.Id).FirstOrDefaultAsync();
                if (existingPenalty != null)
                {
                    existingPenalty.BillingReferenceId = penalty.BillingReferenceId;
                    existingPenalty.Amount = penalty.Amount;
                    existingPenalty.UpdatedBy = userId;
                    existingPenalty.DateUpdated = DateTime.Now;
                    _context.Penalties.Update(existingPenalty);
                }
            }
            await _context.SaveChangesAsync();
            return penalty;
        }

    }
}
