using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;

namespace TMCWD.Data.Services
{
    public class OtherFeeTypeService : IOtherFeeTypeService
    {

        private readonly UserDbContext _context;

        public OtherFeeTypeService(UserDbContext context)
        {
            _context = context;
        }

        public async Task<OtherFeeType> Get(int id)
        {
            var otherFeeType = await _context.OtherFeeTypes.Where(x => x.Id == id).FirstOrDefaultAsync();
            return otherFeeType;
        }

        public async Task<List<OtherFeeType>> GetAll()
        {
            var otherFeeTypes = _context.OtherFeeTypes;
            return await otherFeeTypes.ToListAsync();
        }

        public async Task<List<OtherFeeType>> GetByName(string name)
        {
            var otherFeeTypes = _context.OtherFeeTypes.Where(x => x.Name.Contains(name));
            return await otherFeeTypes.ToListAsync();
        }

        public async Task<OtherFeeType> SaveUpdate(int userId, OtherFeeType otherFeeType)
        {
            otherFeeType.UpdatedBy = userId;
            if(otherFeeType.Id > 0)
            {
                otherFeeType.DateUpdate = DateTime.Now;
                _context.OtherFeeTypes.Update(otherFeeType);
            }
            else
            {
                otherFeeType.CreatedBy = userId;
                otherFeeType.DateCreated = DateTime.Now;
                _context.OtherFeeTypes.Add(otherFeeType);
            }

            await _context.SaveChangesAsync();

            return otherFeeType;
        }

        public async Task<bool> Delete(OtherFeeType otherFeeType)
        {
            _context.OtherFeeTypes.Remove(otherFeeType);
            int res = await _context.SaveChangesAsync();
            if (res > 0) return true;
            return false;
        }
    }

}
