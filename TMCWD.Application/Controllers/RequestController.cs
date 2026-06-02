using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using TMCWD.Administration;
using TMCWD.CustomerSupport;
using TMCWD.Model.Administrator;
using TMCWD.Model.CustomerSupport;
using TMCWD.Services;

namespace TMCWD.Application.Controllers
{
    public class RequestController : Controller
    {
        private readonly HttpClient _client;
        private readonly RequestTransaction _requestTransaction;
        private readonly InspectionTypeTransaction _inspectionTypeTransaction;
        private readonly AuthenticatedUserService _authenticatedUserService;
        private readonly CustomerTransaction _customerTransaction;
        private readonly AccountTransaction _accountTransaction;
        private readonly UserTransaction _userTransaction;

        public RequestController(IHttpClientFactory client, 
            RequestTransaction requestTransaction, 
            AuthenticatedUserService authenticatedUserService, 
            InspectionTypeTransaction inspectionTypeTransaction,
            CustomerTransaction customerTransaction,
            AccountTransaction accountTransaction,
            UserTransaction userTransaction)
        {
            _client = client.CreateClient("TmcWdApi");
            _authenticatedUserService = authenticatedUserService;
            _requestTransaction = requestTransaction;
            _requestTransaction.SetClient(_client);
            _inspectionTypeTransaction = inspectionTypeTransaction;
            _inspectionTypeTransaction.SetClient(_client);
            _customerTransaction = customerTransaction;
            _customerTransaction.SetClient(_client);
            _accountTransaction = accountTransaction;
            _accountTransaction.SetClient(_client);
            _userTransaction = userTransaction;
            _userTransaction.SetClient(_client);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRequest(int customerId, int accountId, bool isDraft, int[]? types = null)
        {
            bool isSuccess = false;
            Request request = new()
            {
                AccountId = accountId,
                Status = isDraft ? RequestStatus.Draft : RequestStatus.InProgress, 
                CustomerId = customerId,
            };

            var createdRequest = await _requestTransaction.SaveUpdate(_authenticatedUserService.User.Id, request);
            if (createdRequest.Id > 0)
            {
                List<RequestDetail> requestDetails = new();
                if(types == null)
                {
                    var newConnectionInspectionType = await _inspectionTypeTransaction.GetNewRequestType();
                    RequestDetail requestDetail = new()
                    {
                        RequestId = createdRequest.Id,
                        RequestTypeId = newConnectionInspectionType.Id,
                    };
                    requestDetails.Add(requestDetail);
                }
                else
                {
                    foreach(var type in types)
                    {
                        RequestDetail requestDetail = new()
                        {
                            RequestId = createdRequest.Id,
                            RequestTypeId = type
                        };
                        requestDetails.Add(requestDetail);
                    }
                }

                var res = await _requestTransaction.SaveMulipleRequestDetail(_authenticatedUserService.User.Id, createdRequest.Id, requestDetails);

                isSuccess = res;
            }

            return Ok(isSuccess);
        }

        [HttpGet]
        public async Task<IActionResult> GetRequests()
        {
            var requests = await _requestTransaction.GetRequests();
            
            if(requests == null || !requests.Any()) return NotFound();

            //var requestInfos = new[]
            //{
            //    new {
            //        Id = 0,
            //        ControlNumber = string.Empty,
            //        AccountId = 0,
            //        CustomerId = 0,
            //        Status = 0,
            //        CustomerName = string.Empty,
            //        AccountNumber = string.Empty,
            //        LoggedDate = DateTime.MinValue,
            //        LoggedBy = string.Empty
            //    }
            //}.Take(0).ToArray();

            List<dynamic> requestInfos = new();

            foreach (var request in requests)
            {
                Task<Customer> getCustomer = _customerTransaction.Get(request.CustomerId);
                Task<Account> getAccount = _accountTransaction.Get(request.AccountId);
                Task<User> getUser = _userTransaction.Get(request.CreatedBy);

                await Task.WhenAll(getCustomer, getAccount, getUser);

                var requestInfo = new
                {
                    Id = request.Id,
                    ControlNumber = request.ControlNumber,
                    AccountId = request.AccountId,
                    CustomerId = request.CustomerId,
                    Status = (int)request.Status,
                    CustomerName = getCustomer.Result.FullName,
                    AccountNumber = getAccount.Result.AccountNumber,
                    LoggedDate = request.DateCreated,
                    LoggedBy = getUser.Result.Name
                };

                requestInfos.Add(requestInfo);
            }

            return Ok(requestInfos.ToList());
        }

    }
}
