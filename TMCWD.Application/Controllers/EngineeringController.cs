using Microsoft.AspNetCore.Mvc;
using TMCWD.Model.Engineering;
using TMCWD.Model.Administrator;
using System.Text.Json;
using TMCWD.Engineering;
using TMCWD.Application.Models;
using TMCWD.Services;

namespace TMCWD.Application.Controllers
{
    public class EngineeringController : Controller
    {

        private readonly HttpClient _client;
        private readonly AuthenticatedUserService _authenticatedUserService;
        private readonly InventoryTransaction _inventoryTransaction;

        public EngineeringController(IHttpClientFactory factory, AuthenticatedUserService authenticatedUserService, InventoryTransaction inventoryTransaction)
        {
            _client = factory.CreateClient("TmcWdApi");
            _authenticatedUserService = authenticatedUserService;
            _inventoryTransaction = inventoryTransaction;
            _inventoryTransaction.SetClient(_client);
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Inventory()
        {
            User currentUser = _authenticatedUserService.User;

            InventoryViewModel model = new();
            model.CurrentUser = currentUser;

            model.Inventory = await _inventoryTransaction.GetAll();

            return View(model);
        }

        public async Task<IActionResult> AddEditInventory(int inventoryId = 0)
        {
            User currentUser = _authenticatedUserService.User;
            InventoryViewModel model = new();
            model.CurrentUser = currentUser;

            if(inventoryId > 0)
            {
                model.AddEditInventory = new();
                model.AddEditInventory = await _inventoryTransaction.Get(inventoryId);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveUpdateInventory(InventoryViewModel model)
        {
            await _inventoryTransaction.SaveUpdate(_authenticatedUserService.User.Id, model.AddEditInventory);
            return RedirectToAction("Inventory", "Engineering");
        }

        public async Task<IActionResult> UpdateInventoryQuantity(int itemId, int newQuantity, int currentUserId)
        {
            Inventory inv = new();
            inv = await _inventoryTransaction.Get(itemId);
            if(inv != null)
            {
                if(inv.Quantity != newQuantity)
                {
                    inv.Quantity = newQuantity;
                    inv.UpdatedBy = currentUserId;
                    await _inventoryTransaction.SaveUpdate(_authenticatedUserService.User.Id, inv);
                }
            }
            return RedirectToAction("Inventory", "Engineering");
        }

    }
}
