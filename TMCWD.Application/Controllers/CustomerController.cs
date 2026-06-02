using Microsoft.AspNetCore.Mvc;
using TMCWD.CustomerSupport;
using TMCWD.Model.CustomerSupport;
using TMCWD.Services;

namespace TMCWD.Application.Controllers
{
    public class CustomerController : Controller
    {

        private readonly HttpClient _client;
        private readonly AuthenticatedUserService _authenticatedUserService;
        private readonly CustomerTransaction _customerTransaction;

        public CustomerController(IHttpClientFactory client, AuthenticatedUserService authenticatedUserService, CustomerTransaction customerTransaction)
        {
            _client = client.CreateClient("TmcWdApi");
            _authenticatedUserService = authenticatedUserService;
            _customerTransaction = customerTransaction;
            _customerTransaction.SetClient(_client);
        }

        [HttpPost]
        public async Task<IActionResult> SaveUpdateCustomer(Customer customer)
        {
            var updateCustomer = await _customerTransaction.SaveUpdate(_authenticatedUserService.User.Id, customer);
            if (customer == null) return BadRequest();
            return Ok(updateCustomer);
        }
    }
}
