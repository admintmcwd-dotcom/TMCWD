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

        private readonly HttpClient _client;

        public CustomerSupportController(IHttpClientFactory factory)
        {
            _client = factory.CreateClient("TmcWdApi");
        }

        public async Task<IActionResult> Index()
        {
            CustomerViewModel model = new();
            UserTransaction userTrans = new(_client);
            User currentUser = new();
            currentUser = userTrans.ConvertJsonStringToUser(HttpContext.Session.GetString("currentUser"));

            CustomerTransaction custTrans = new(_client);

            model.CurrentUser = currentUser ?? new User();
            model.PagedCustomerList = await custTrans.GetCustomers();
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

        [HttpPost]
        public IActionResult SaveRequest([FromBody] object data)
        {
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            User currentUser = new();
            string stringRequest = data.ToString();

            List<int> detailIds = new List<int>();

            if (!String.IsNullOrEmpty(jsonCurrentUser.Trim()))
            {
                currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
            }

            if (String.IsNullOrEmpty(stringRequest.Trim())) return BadRequest("Request data is empty");

            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            Request request = JsonSerializer.Deserialize<Request>(stringRequest, serializerOptions);

            if (request == null) return BadRequest("Cannot convert request data to request object");

            if (request.Id > 0)
            {
                request.DateUpdated = DateTime.Now;
                request.UpdatedBy = currentUser.Id;
            }
            else
            {
                request.DateCreated = DateTime.Now;
                request.CreatedBy = currentUser.Id;

            }
            request.AccountId = 1;
            request.CustomerId = 1;
            request.ControlNumber = "TKT";

            RequestTransaction requestTrans = new();
            int id = requestTrans.SaveUpdate(request);

            return Ok(id);
        }

        [HttpPost]
        public IActionResult SaveRequestDetails(int requestId, [FromBody] object data)
        {
            var jsonCurrentUser = HttpContext.Session.GetString("currentUser");
            User currentUser = new();
            string stringRequest = data.ToString();

            List<int> detailIds = new List<int>();

            if (!String.IsNullOrEmpty(jsonCurrentUser.Trim()))
            {
                currentUser = JsonSerializer.Deserialize<User>(jsonCurrentUser);
            }

            if (data == null) return BadRequest("No selected service(s)");

            string listIds = data.ToString();

            if (String.IsNullOrEmpty(listIds.Trim())) return BadRequest("No selected service(s)");

            var serializeOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            List<RequestDetail> requestDetails = JsonSerializer.Deserialize<List<RequestDetail>>(listIds);

            if (requestDetails == null || !requestDetails.Any()) return BadRequest("One of the services id is not in the correct format");

            foreach (var requestDetail in requestDetails)
            {
                requestDetail.RequestId = requestId;
            }

            RequestTransaction requestTrans = new();

            var res = requestTrans.SaveMulipleRequestDetail(requestDetails);

            return Ok(res);
        }

    }
}
