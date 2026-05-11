using Microsoft.AspNetCore.Mvc;
using TMCWD.Model.Engineering;
using TMCWD.Model.Administrator;
using System.Text.Json;
using TMCWD.Engineering;
using TMCWD.Application.Models;

namespace TMCWD.Application.Controllers
{
    public class EngineeringController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Inventory()
        {
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            User currentUser = new();

            InventoryViewModel model = new();
            
            if (!String.IsNullOrEmpty(jsonCurrentUser.Trim()))
            {
                currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
                model.CurrentUser = currentUser;
                ViewBag.Role = currentUser.Role;
            }

            InventoryTransaction invTrans = new();
            model.Inventory = invTrans.GetAll();

            return View(model);
        }

        public IActionResult AddEditInventory(int inventoryId = 0)
        {
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            User currentUser = new();
            InventoryViewModel model = new();

            if (!String.IsNullOrEmpty(jsonCurrentUser.Trim()))
            {
                currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
                model.CurrentUser = currentUser;
                ViewBag.Role = currentUser?.Role;
            }

            if(inventoryId > 0)
            {
                model.AddEditInventory = new();
                InventoryTransaction invTrans = new();
                model.AddEditInventory = invTrans.GetById(inventoryId);
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult SaveUpdateInventory(InventoryViewModel model)
        {
            InventoryTransaction invTrans = new();
            if (model.AddEditInventory.Id > 0) model.AddEditInventory.UpdatedBy = model.CurrentUser.Id;
            else model.AddEditInventory.CreatedBy = model.CurrentUser.Id;
            invTrans.SaveUpdate(model.AddEditInventory);
            return RedirectToAction("Inventory", "Engineering");
        }

        public IActionResult UpdateInventoryQuantity(int itemId, int newQuantity, int currentUserId)
        {
            InventoryTransaction invTrans = new();
            Inventory inv = new();
            inv = invTrans.GetById(itemId);
            if(inv != null)
            {
                if(inv.Quantity != newQuantity)
                {
                    inv.Quantity = newQuantity;
                    inv.UpdatedBy = currentUserId;
                    invTrans.SaveUpdate(inv);
                }
            }
            return RedirectToAction("Inventory", "Engineering");
        }

    }
}
