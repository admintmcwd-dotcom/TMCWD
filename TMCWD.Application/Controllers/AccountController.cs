using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TMCWD.Application.Models;
using TMCWD.CustomerSupport;
using TMCWD.Model.Administrator;
using TMCWD.Model.CustomerSupport;
using TMCWD.Services;

namespace TMCWD.Application.Controllers
{
    public class AccountController : Controller
    {

        private readonly HttpClient _client;
        private readonly AuthenticatedUserService _authenticatedUserService;
        private readonly CustomerTransaction _customerTransaction;
        private readonly AccountTransaction _accountTransaction;

        public AccountController(IHttpClientFactory factory, AuthenticatedUserService authenticatedUserService, CustomerTransaction customerTransaction, AccountTransaction accountTransaction)
        {
            _client = factory.CreateClient("TmcWdApi");
            _authenticatedUserService = authenticatedUserService;
            _customerTransaction = customerTransaction;
            _customerTransaction.SetClient(_client);
            _accountTransaction = accountTransaction;
            _accountTransaction.SetClient(_client);
        }

        public async Task<IActionResult> Index(int customerId, int accountId = 0)
        {

            User currentUser = _authenticatedUserService.User;
            ViewBag.Role = currentUser.Role;

            AccountViewModel model = new(customerId);
            model.Customer = await _customerTransaction.Get(customerId);
            model.PagedAccountList = await _accountTransaction.GetByCustomerId(customerId);
            model.CurrentUser = currentUser;

            if(accountId > 0)
            {
                model.AddEditAccount = await _accountTransaction.Get(accountId);
            }
            else model.AddEditAccount.CustomerId = customerId;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveAccount(AccountViewModel model)
        {
            User currentUser = _authenticatedUserService.User;

            await _accountTransaction.SaveUpdate(currentUser.Id, model.AddEditAccount);
            return RedirectToAction("Index", "Account", new { customerId = model.AddEditAccount.CustomerId });
        }

        public async Task<IActionResult> DeactivateAccount(int accountId)
        {
            Account acct = new();
            acct =  await _accountTransaction.Get(accountId);
            if(acct != null)
            {
                acct.Status = AccountStatus.Closed;
                await _accountTransaction.SaveUpdate(_authenticatedUserService.User.Id, acct);
                RedirectToAction("Index", "Account", new { customerId = acct.CustomerId });
            }
            return RedirectToAction("Index", "Account");
        }

    }
}
