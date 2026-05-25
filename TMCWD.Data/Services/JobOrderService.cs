using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;

namespace TMCWD.Data.Services
{
    public class JobOrderService : IJobOrderService
    {
        private readonly UserDbContext _context;

        public JobOrderService(UserDbContext context)
        {
            _context = context;
        }

        public async Task<JobOrder> Get(int id)
        {
            var jobOrder = await _context.JobOrders.Where(x => x.Id == id).FirstOrDefaultAsync();
            return jobOrder;
        }

        public async Task<List<JobOrder>> GetAll(int requestId)
        {
            var jobOrders = _context.JobOrders.Where(x => x.RequestId == requestId);
            return await jobOrders.ToListAsync();
        }

        public async Task<JobOrder> SaveUpdate(int userId, JobOrder jobOrder)
        {
            jobOrder.DateUpdated = DateTime.Now;
            if(jobOrder.Id > 0)
            {
                jobOrder.UpdatedBy = userId;
                _context.JobOrders.Update(jobOrder);
            }
            else
            {
                jobOrder.CreatedBy = userId;
                jobOrder.DateCreated = DateTime.Now;
            }

            await _context.SaveChangesAsync();
            return jobOrder;
        }
    }
}
