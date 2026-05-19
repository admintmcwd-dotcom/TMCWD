using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using TMCWD.Data.Context;
using TMCWD.Data.Entities;
using TMCWD.Data.Services;
using TMCWD.Utility.Generic;

namespace TMCWD.Data.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryController : Controller
    {

        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService service) { _inventoryService = service; }

        [HttpPost("SaveUpdate/{userId}")]
        public async Task<ActionResult<Inventory>> SaveUpdate(int userId, [FromBody] Inventory inventory)
        {
            StringBuilder sb = new();
            if (String.IsNullOrEmpty(inventory.Name.Trim())) sb.AppendLine("Inventory item name is required to create inventory");
            if (inventory.Quantity <= 0) sb.AppendLine("Quantity must be greater than zero (0)");
            if(inventory.UnitCost <= 0) sb.AppendLine("Unit cost must be greater than zero (0)");
            if (String.IsNullOrEmpty(inventory.UOM.Trim())) sb.AppendLine("Please specify unit of measurement (UOM)");

            if (String.IsNullOrEmpty(sb.ToString().Trim())) return BadRequest(sb.ToString());

            var updatedInventory = await _inventoryService.SaveUpdate(userId, inventory);
            if (inventory == null) return BadRequest("Problem(s) encountered while saving inventory item");
            return Ok(updatedInventory);
        }

        [HttpGet("Get/{id}")]
        public async Task<ActionResult<Inventory>> Get(int id)
        {
            var inventory = await _inventoryService.Get(id);
            if (inventory == null) return NotFound($"Inventory item with id {id} was not found.");
            return Ok(inventory);
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetAll()
        {
            var inventory = await _inventoryService.GetAll();
            if (inventory == null || !inventory.Any()) return NotFound("Inventory item(s) not found.");
            return Ok(inventory);
        }

        [HttpGet("GetByName/{name}")]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetByName(string name)
        {
            var inventory = await _inventoryService.GetByName(name);
            if (inventory == null || !inventory.Any()) return NotFound($"Inventory item with name {name} was not found.");
            return Ok(inventory);
        }

    }
}
