using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TMCWD.Application.Models;
using TMCWD.CustomerSupport;
using TMCWD.Model.Administrator;

namespace TMCWD.Application.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index(int customerId, int accountId = 0)
        {

            string? jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            User? currentUser = new();

            if (!String.IsNullOrEmpty(jsonCurrentUser?.Trim()))
            {
                currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
                ViewBag.Role = currentUser?.Role;
            }

            AccountViewModel model = new(customerId);
            model.GetCustomerDetails();
            model.GetAccounts();
            model.CurrentUser = currentUser;

            if(accountId > 0)
            {
                AccountTransaction acctTrans = new();
                model.AddEditAccount = acctTrans.GetById(accountId);
            }
            else model.AddEditAccount.CustomerId = customerId;

            return View(model);
        }

        [HttpPost]
        public IActionResult SaveAccount(AccountViewModel model)
        {
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            User currentUser = new();

            if (!String.IsNullOrEmpty(jsonCurrentUser.Trim()))
            {
                currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
                if (model.AddEditAccount.Id <= 0)
                {
                    model.AddEditAccount.AccountNumber = model.AddEditAccount.MeterNumber;
                    model.AddEditAccount.CreatedBy = currentUser.Id;
                    model.AddEditAccount.DateCreated = DateTime.Now;
                }
            }
            model.AddEditAccount.DateUpdated = DateTime.Now;
            model.AddEditAccount.UpdatedBy = currentUser.Id;

            AccountTransaction acctTrans = new();
            acctTrans.SaveUpdate(model.AddEditAccount);
            return RedirectToAction("Index", "Account", new { customerId = model.AddEditAccount.CustomerId });
        }

    }
}
