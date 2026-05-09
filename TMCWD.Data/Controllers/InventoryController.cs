using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : Controller
    {

        private readonly UserDbContext _dbContext;

        public InventoryController(UserDbContext dbContext) { _dbContext = dbContext; }

        [HttpPost("SaveUpdate")]
        public ActionResult<bool> SaveUpdate([FromBody] Inventory inventory)
        {

            if (inventory.Id > 0) _dbContext.Inventories.Update(inventory);
            else _dbContext.Inventories.Add(inventory);

            int res = _dbContext.SaveChanges();

            if (res > 0) return Ok(true);

            return Ok(false);
        }

        [HttpGet("GetById")]
        public ActionResult<Inventory> GetById(int id)
        {

            Inventory inventory = new();

            var data = _dbContext.Inventories.FirstOrDefault(x => x.Id == id);
            if (data == null) return NotFound($"Inventory with id {id} is not found.");
            inventory = data;

            return Ok(inventory);
        }

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<Inventory>> GetAll()
        {

            IEnumerable<Inventory> inventories = new List<Inventory>();

            var data = _dbContext.Inventories;
            if (data == null || !data.Any()) return NotFound($"No inventory found.");
            inventories = data;

            return Ok(inventories);
        }

        [HttpGet("GetByName")]
        public ActionResult<IEnumerable<Inventory>> GetByName(string name)
        {
            IEnumerable<Inventory> inventories = new List<Inventory>();

            var data = _dbContext.Inventories.Where(x => x.Name.Contains(name));
            if (data == null || !data.Any()) return NotFound(null);
            inventories = data;

            return Ok(inventories);
        }

    }
}
