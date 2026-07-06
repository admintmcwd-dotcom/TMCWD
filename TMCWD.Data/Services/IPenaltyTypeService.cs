using TMCWD.Data.Entities;

namespace TMCWD.Data.Services
{
    public interface IPenaltyTypeService
    {

        public Task<PenaltyType> Get(int id);

        public Task<List<PenaltyType>> GetAll();

        public Task<PenaltyType> SaveUpdate(int userId, PenaltyType penaltyType);

    }
}
