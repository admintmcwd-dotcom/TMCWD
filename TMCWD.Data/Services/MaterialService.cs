using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;

namespace TMCWD.Data.Services
{
    public class MaterialService : IMaterialService
    {

        private readonly UserDbContext _context;

        public MaterialService(UserDbContext context)
        {
            _context = context;
        }

        public async Task<Material> Get(int id)
        {
            var material = await _context.Materials.Where(x => x.Id == id).FirstOrDefaultAsync();
            return material;
        }

        public async Task<List<Material>> GetAll()
        {
            var material = _context.Materials;
            return await material.ToListAsync();
        }

        public async Task<List<Material>> GetByRequestId(int requestId)
        {
            var materials = _context.Materials.Where(x => x.RequestId == requestId);
            return await materials.ToListAsync();
        }

        public async Task<Material> SaveUpdate(int userId, int requestId, Material material)
        {
            material.DateUpdated = DateTime.Now;
            if(material.Id > 0)
            {
                material.UpdatedBy = userId;
                _context.Materials.Update(material);
            }
            else
            {
                material.RequestId = requestId;
                material.DateCreated = DateTime.Now;
                material.CreatedBy = userId;
                _context.Materials.Add(material);
            }

            await _context.SaveChangesAsync();
            return material;
        }

        public async Task<Material> UpdateQuantityOrNewUnitCost(int userId, int requestId, Material updateMaterial)
        {
            var material = _context.Materials.Where(x => x.RequestId == updateMaterial.RequestId && x.Id == updateMaterial.Id).FirstOrDefault();

            if (material == null) return null;

            material.UpdatedBy = userId;
            material.DateUpdated = DateTime.Now;
            if(material.RequestedQuantity !=  updateMaterial.RequestedQuantity) material.RequestedQuantity = updateMaterial.RequestedQuantity;
            if (material.NewUnitCost != updateMaterial.NewUnitCost) material.NewUnitCost = updateMaterial.NewUnitCost;
            _context.Materials.Update(material);

            await _context.SaveChangesAsync();

            return material;
        }

    }

}
