using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TMCWD.Administration;
using TMCWD.Application.Models;
using TMCWD.Model.Administrator;
using TMCWD.Model.CustomerSupport;
using TMCWD.CustomerSupport;
using TMCWD.Services;
using System.Net;

namespace TMCWD.Application.Controllers
{
    public class CustomerSupportController : Controller
    {

        #region fields

        private readonly HttpClient _client;
        private readonly AuthenticatedUserService _authenticatedUserService;
        private readonly AccountTransaction _accountTransaction;
        private readonly CustomerTransaction _customerTransaction;
        private readonly RequestTransaction _requestTransaction;
        private readonly InspectionTypeTransaction _inspectionTypeTransaction;
        private readonly UserTransaction _userTransaction;
        private readonly RecommendationTransaction _recommendationTransaction;

        #endregion

        #region constructors

        public CustomerSupportController(IHttpClientFactory factory, 
            AuthenticatedUserService authenticatedUserService, 
            AccountTransaction accountTransaction, 
            CustomerTransaction customerTransaction,
            RequestTransaction requestTransaction,
            InspectionTypeTransaction inspectionTypeTransaction,
            UserTransaction userTransaction,
            RecommendationTransaction recommendationTrasaction)
        {
            _client = factory.CreateClient("TmcWdApi");
            _authenticatedUserService = authenticatedUserService;

            _accountTransaction = accountTransaction;
            _accountTransaction.SetClient(_client);

            _customerTransaction = customerTransaction;
            _customerTransaction.SetClient(_client);

            _requestTransaction = requestTransaction;
            _requestTransaction.SetClient(_client);

            _inspectionTypeTransaction = inspectionTypeTransaction;
            _inspectionTypeTransaction.SetClient(_client);

            _userTransaction = userTransaction;
            _userTransaction.SetClient(_client);

            _recommendationTransaction = recommendationTrasaction;
            _recommendationTransaction.SetClient(_client);
        }

        #endregion

        #region public methods

        public async Task<IActionResult> Index()
        {
            CustomerViewModel model = new();
            User currentUser = _authenticatedUserService.User;

            model.CurrentUser = currentUser ?? new User();
            model.PagedCustomerList = await _customerTransaction.GetCustomers();
            ViewBag.Role = model.CurrentUser.Role;

            return View(model);
        }

        public async Task<IActionResult> AddEditCustomer(int editCustomerId = 0)
        {
            User currentUser = _authenticatedUserService.User;

            CustomerViewModel model = new()
            {
                CurrentUser = currentUser,
                AddEditCustomer = await _customerTransaction.Get(editCustomerId) ?? new Customer(),
                CustomerAccounts = editCustomerId > 0 ? await _accountTransaction.GetByCustomerId(editCustomerId) : new List<Account>()
            };

            ViewBag.Role = currentUser.Role;

            return View(model);
        }

        public IActionResult AddEditAccount()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SaveCustomer(CustomerViewModel model)
        {

            if(model.AddEditCustomer == null) return BadRequest("Customer data is required");

            await _customerTransaction.SaveUpdate(_authenticatedUserService.User.Id, model.AddEditCustomer);

            return RedirectToAction("Index", "CustomerSupport");
        }

        public async Task<IActionResult> DeactivateCustomer(int customerId)
        {
            Customer cust = new();
            User currentUser = new();
            currentUser = _authenticatedUserService.User;
            cust = await _customerTransaction.Get(customerId);

            if(cust != null)
            {
                cust.IsActive = false;
                await _customerTransaction.SaveUpdate(currentUser.Id, cust);
            }

            return RedirectToAction("Index", "CustomerSupport");
        }

        public async Task<IActionResult> DeactivateAccount(int accountId)
        {
            AccountTransaction acctTrans = new();
            Account acct = new();
            acct = await _accountTransaction.Get(accountId);

            if(acct != null)
            {
                acct.Status = AccountStatus.Closed;
                await acctTrans.SaveUpdate(_authenticatedUserService.User.Id, acct);
            }

            return RedirectToAction("AddEditCustomer", "CustomerSupport", new { editCustomerId = acct.CustomerId });
        }

        public async Task<IActionResult> Requests()
        {
            User currentUser = _authenticatedUserService.User;
            RequestViewModel model = new();
            model.CurrentUser = currentUser;
            ViewBag.Role = currentUser.Role;
            model.Requests = await _requestTransaction.GetRequests();
            model.CustomerTransaction =_customerTransaction;
            model.AccountTransaction = _accountTransaction;
            return View(model);
        }

        public async Task<IActionResult> AddEditRequest(int requestId = 0)
        {
            User currentUser = _authenticatedUserService.User;
            RequestViewModel model = new();
            model.CurrentUser = currentUser;
            ViewBag.Role = currentUser?.Role;

            var types = await _inspectionTypeTransaction.GetTypes() ?? new();

            List<RequestDetail> requestDetails = new();

            if (requestId > 0)
            {
                model.AddEditRequest = await _requestTransaction.Get(requestId);
                if(model.AddEditRequest != null)
                {
                    Task<Customer> getCustomerTask = _customerTransaction.Get(model.AddEditRequest.CustomerId);
                    Task<Account> getAccountTask = _accountTransaction.Get(model.AddEditRequest.AccountId);
                    Task<List<RequestDetail>> getRequestDetailsTask = _requestTransaction.GetRequestDetailByRequestId(model.AddEditRequest.Id);
                    Task<List<Recommendation>> getRecommendationsTask = _recommendationTransaction.GetByRequestId(model.AddEditRequest.Id);

                    await Task.WhenAll(getCustomerTask, getAccountTask, getRequestDetailsTask, getRecommendationsTask);
                    model.CurrentCustomer = getCustomerTask.Result;
                    model.CurrentAccount = getAccountTask.Result;
                    requestDetails = getRequestDetailsTask.Result;
                    model.Recommendations = getRecommendationsTask.Result;

                    //model.CurrentCustomer = await _customerTransaction.Get(model.AddEditRequest.CustomerId);
                    //model.CurrentAccount = await _accountTransaction.Get(model.AddEditRequest.AccountId);
                    //requestDetail = await _requestTransaction.GetRequestDetailByRequestId(model.AddEditRequest.Id) ?? new();
                    //model.Recommendations = await _recommendationTransaction.GetByRequestId(model.AddEditRequest.Id) ?? new();
                }
            }

            model.InspectionTypes = ConvertToInspectionTypeToViewModel(types, requestDetails);

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {

            Customer customer = new();
            customer = await _customerTransaction.Get(customerId);

            return View(customer);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserById(int userId)
        {
            User user = new();
            user = await _userTransaction.Get(userId);
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRequest([FromBody] object data)
        {
            User currentUser = _authenticatedUserService.User;
            string? stringRequest = data?.ToString();

            List<int> detailIds = new List<int>();

            if (String.IsNullOrEmpty(stringRequest?.Trim())) return BadRequest("Request data is empty");

            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            Request? request = JsonSerializer.Deserialize<Request>(stringRequest, serializerOptions);

            if (request == null) return BadRequest("Cannot convert request data to request object");

            request.ControlNumber = "TKT";

            var updatedRequest = await _requestTransaction.SaveUpdate(currentUser.Id, request);

            return Ok(updatedRequest.Id);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRequestDetails(int requestId, [FromBody] object data)
        {
            User currentUser = _authenticatedUserService.User;
            string stringRequest = data.ToString();

            List<int> detailIds = new List<int>();

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

            var res = await _requestTransaction.SaveMulipleRequestDetail(currentUser.Id, requestId, requestDetails);

            return Ok(res);
        }

        public async Task<IActionResult> GetCustomerName(int customerId)
        {
            var customer = await _customerTransaction.Get(customerId);
            return ViewComponent("CustomerName", $"{customer.Firstname} {customer.Lastname}");
        }

        [HttpGet]
        public async Task<IActionResult> SearchCustomer(string searchString)
        {
            var customers = await _customerTransaction.Search(searchString);
            if (customers == null || !customers.Any()) return NotFound();
            return Ok(customers);
        }

        [HttpPost]
        public async Task<IActionResult> SaveRecommendation([FromBody] object data)
        {

            if (data == null) return NoContent();
            string stringRequest = JsonSerializer.Serialize(data);

            if (String.IsNullOrEmpty(stringRequest.Trim())) return NoContent();

            using var doc = JsonDocument.Parse(stringRequest);
            if (doc == null) return NoContent();

            JsonElement root = doc.RootElement;

            if(root.ValueKind == JsonValueKind.Null) return NoContent();
            var requestIdAttr = root.GetProperty("requestId");
            var detailsAttr = root.GetProperty("details");

            if(requestIdAttr.ValueKind == JsonValueKind.Null || detailsAttr.ValueKind == JsonValueKind.Null) return NoContent();
            int.TryParse(requestIdAttr.GetString(), out int requestId);
            string details = detailsAttr.GetString();

            Recommendation recommendation = new()
            {
                RequestId = requestId,
                Details = WebUtility.UrlDecode(details)
            };

            recommendation.DateCreated = DateTime.Now;
            recommendation.DateUpdated = DateTime.Now;

            var updatedRecommendation = await _recommendationTransaction.SaveUpdate(recommendation.RequestId, _authenticatedUserService.User.Id, recommendation);

            return Ok(updatedRecommendation);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByIdForTable(int userId)
        {
            User user = new();
            user = await _userTransaction.Get(userId);
            if (user == null) return NoContent();
            return Ok(user);
        }

        public IActionResult RefreshCustomerNameComponent(int customerId, string pId = "", string pClass = "")
        {
            return ViewComponent("CustomerName", new { customerId = customerId, pId = pId, pClass = pClass });
        }

        public IActionResult RefreshAccountSelectComponent(int customerId)
        {
            return ViewComponent("AccountSelection", new { customerId = customerId });
        }

        #endregion

        #region private methods

        private List<RequestInspectionTypeViewModel> ConvertToInspectionTypeToViewModel(List<InspectionType> inspTypes, List<RequestDetail> details = null)
        {

            List<RequestInspectionTypeViewModel> types = new();

            if (inspTypes == null || inspTypes.Count <= 0) return new List<RequestInspectionTypeViewModel>();

            foreach(var inspType in inspTypes)
            {
                RequestInspectionTypeViewModel model = new RequestInspectionTypeViewModel
                {
                    Type = inspType,
                    IsChecked = details.Where(x => x.RequestTypeId == inspType.Id).Any()

                };

                types.Add(model);
            }

            return types;
        }

        #endregion

    }

}
