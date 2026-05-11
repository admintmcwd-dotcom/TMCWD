using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TMCWD.Administration;
using TMCWD.Application.Models;
using TMCWD.Model.Administrator;
using TMCWD.Model.CustomerSupport;
using TMCWD.CustomerSupport;

namespace TMCWD.Application.Controllers
{
    public class CustomerSupportController : Controller
    {
        public IActionResult Index()
        {
            CustomerViewModel model = new();
            User? currentUser = new();
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");

            CustomerTransaction custTrans = new();

            if (!String.IsNullOrEmpty(jsonCurrentUser?.Trim()))
            {
                currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
            }

            model.CurrentUser = currentUser ?? new User();
            model.PagedCustomerList = custTrans.GetCustomers();
            ViewBag.Role = model.CurrentUser.Role;

            return View(model);
        }

        public IActionResult AddEditCustomer(int editCustomerId = 0)
        {
            User? currentUser = new();
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");

            if (!String.IsNullOrEmpty(jsonCurrentUser?.Trim()))
            {
                currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
            }

            AccountTransaction acctTrans = new();
            CustomerTransaction custTrans = new();

            CustomerViewModel model = new()
            {
                CurrentUser = currentUser,
                AddEditCustomer = custTrans.GetById(editCustomerId) ?? new Customer(),
                CustomerAccounts = editCustomerId > 0 ? acctTrans.GetByCustomerId(editCustomerId) : new List<Account>()
            };

            ViewBag.Role = currentUser?.Role;

            return View(model);
        }

        public IActionResult AddEditAccount()
        {

            return View();
        }

        [HttpPost]
        public IActionResult SaveCustomer(CustomerViewModel model)
        {
            CustomerTransaction custTrans = new();
            model.AddEditCustomer.CreatedBy = model.CurrentUser.Id;
            custTrans.SaveUpdate(model.AddEditCustomer);

            return RedirectToAction("Index", "CustomerSupport");
        }

        public IActionResult DeactivateCustomer(int customerId)
        {
            CustomerTransaction custTrans = new();
            Customer cust = new();
            cust = custTrans.GetById(customerId);

            if(cust != null)
            {
                cust.IsActive = false;
                custTrans.SaveUpdate(cust);
            }

            return RedirectToAction("Index", "CustomerSupport");
        }

        public IActionResult DeactivateAccount(int accountId)
        {
            AccountTransaction acctTrans = new();
            Account acct = new();
            acct = acctTrans.GetById(accountId);

            if(acct != null)
            {
                acct.IsActive = false;
                acctTrans.SaveUpdate(acct);
            }

            return RedirectToAction("AddEditCustomer", "CustomerSupport", new { editCustomerId = acct.CustomerId });
        }

        public IActionResult Requests()
        {
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            User currentUser = new();

            UserTransaction trans = new();
            RequestTransaction requestTrans = new();
            RequestViewModel model = new();
            if (!String.IsNullOrEmpty(jsonCurrentUser.Trim()))
            {
                currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
                model.CurrentUser = currentUser;
                ViewBag.Role = currentUser.Role;
            }

            model.Requests = requestTrans.GetRequests();

            return View(model);
        }

        public IActionResult AddEditRequest(int requestId = 0)
        {
            string jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            User currentUser = new();
            RequestViewModel model = new();
            if (!String.IsNullOrEmpty(jsonCurrentUser.Trim()))
            {
                currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
                model.CurrentUser = currentUser;
                ViewBag.Role = currentUser?.Role;
            }

            if(requestId > 0)
            {
                RequestTransaction requestTrans = new();
                model.AddEditRequest = requestTrans.GetById(requestId);
            }

            InspectionTypeTransaction inspTrans = new();
            model.InspectionTypes = inspTrans.GetIncidentTypes() ?? new();

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {

            CustomerTransaction custTrans = new();
            Customer customer = new();
            customer = custTrans.GetById(customerId);

            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(int userId)
        {
            UserTransaction userTrans = new();
            User user = new();
            user = userTrans.GetUserById(userId);
            return View(user);
        }

    }
}
