using TMCWD.Data.Entities;

namespace TMCWD.Data.Services
{
    public interface IMaterialService
    {

        Task<Material> Get(int id);
        Task<List<Material>> GetAll();
        Task<List<Material>> GetByRequestId(int requestId);
        Task<Material> SaveUpdate(int userId, int requestId, Material material);
        Task<Material> UpdateQuantityOrNewUnitCost(int userId, int requestId, Material updateMaterial);

    }
}
