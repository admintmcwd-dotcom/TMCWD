using TMCWD.Data.Entities;

namespace TMCWD.Data.Services
{
    public interface IJobOrderService
    {
        Task<JobOrder> Get(int id);
        Task<List<JobOrder>> GetAll(int requestId);
        Task<JobOrder> SaveUpdate(int userId, JobOrder jobOrder);
    }
}
