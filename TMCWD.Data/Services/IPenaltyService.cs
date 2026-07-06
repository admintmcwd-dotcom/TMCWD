using TMCWD.Data.Entities;

namespace TMCWD.Data.Services
{
    public interface IPenaltyService
    {

        public Task<Penalty> Get(int id);

        public Task<List<Penalty>> GetAll();

        public Task<List<Penalty>> GetByBillingReferenceId(string billingReferenceId);

        public Task<Penalty> SaveUpdate(int userId, Penalty penalty);

    }
}
