using Microsoft.AspNetCore.Mvc;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : Controller
    {

        [HttpPost("SaveUpdate")]
        public ActionResult<bool> SaveUpdate([FromBody] Inventory inventory)
        {

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    if (inventory.Id > 0) dbContext.Inventories.Update(inventory);
                    else dbContext.Inventories.Add(inventory);

                    int res = dbContext.SaveChanges();

                    if (res > 0) return Ok(true);
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(false);
        }

        [HttpGet("GetById")]
        public ActionResult<Inventory> GetById(int id)
        {

            Inventory inventory = new();

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var data = dbContext.Inventories.FirstOrDefault(x => x.Id == id);
                    if (data == null) return NotFound($"Inventory with id {id} is not found.");
                    inventory = data;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(inventory);
        }

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<Inventory>> GetAll()
        {

            IEnumerable<Inventory> inventories = new List<Inventory>();

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var data = dbContext.Inventories;
                    if (data == null || !data.Any()) return NotFound($"No inventory found.");
                    inventories = data;
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(inventories);
        }

        [HttpGet("GetByName")]
        public ActionResult<IEnumerable<Inventory>> GetByName(string name)
        {
            IEnumerable<Inventory> inventories = new List<Inventory>();

            try
            {
                using (var dbContext = new UserDbContext())
                {
                    var data = dbContext.Inventories.Where(x => x.Name.Contains(name));
                    if (data == null || !data.Any()) return NotFound(null);
                    inventories = data;
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Data, ErrorType.Error, ex.Message);
                return Problem(ex.Message, ErrorModule.Data.ToString(), StatusCodes.Status500InternalServerError, ErrorType.Error.ToString(), ErrorType.Error.ToString());
            }

            return Ok(inventories);
        }

    }
}
