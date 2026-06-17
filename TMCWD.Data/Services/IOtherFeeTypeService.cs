using TMCWD.Data.Entities;

namespace TMCWD.Data.Services
{
    public interface IOtherFeeTypeService
    {

        Task<OtherFeeType?> Get(int id);
        Task<List<OtherFeeType>> GetByName(string name);
        Task<List<OtherFeeType>> GetAll();
        Task<OtherFeeType> SaveUpdate(int userId, OtherFeeType feeType);
        Task<bool> Delete(OtherFeeType otherFeeType);
    }
}
