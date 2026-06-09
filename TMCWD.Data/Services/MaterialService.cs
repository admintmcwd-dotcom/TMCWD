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

        public async Task<Material?> Get(int id)
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

            updateMaterial.UpdatedBy = userId;
            updateMaterial.DateUpdated = DateTime.Now;

            _context.Materials.Update(updateMaterial);

            await _context.SaveChangesAsync();

            return updateMaterial;
        }

    }

}
