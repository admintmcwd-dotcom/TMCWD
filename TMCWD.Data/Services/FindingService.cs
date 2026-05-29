using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;

namespace TMCWD.Data.Services
{
    public class FindingService : IFindingService
    {

        private readonly UserDbContext _context;

        public FindingService(UserDbContext context)
        {
            _context = context;
        }

        public async Task<Finding> Get(int id)
        {
            var finding = await _context.Findings.Where(x => x.Id == id).FirstOrDefaultAsync();
            return finding;
        }

        public async Task<IEnumerable<Finding>> GetAll(int requestId)
        {
            var findings = _context.Findings.Where(x => x.RequestId == requestId);
            return await findings.ToListAsync();
        }

        public async Task<Finding> SaveUpdate(int userId, int requestId, Finding finding)
        {
            finding.DateUpdated = DateTime.Now;
            if(finding.Id > 0)
            {
                finding.UpdatedBy = userId;
                finding.RequestId = requestId;
                _context.Findings.Update(finding);
            }
            else
            {
                finding.DateCreated = DateTime.Now;
                finding.CreatedBy = userId;
                finding.RequestId = requestId;
                _context.Findings.Add(finding);
            }

            await _context.SaveChangesAsync();

            return finding;
        }
    }
}
