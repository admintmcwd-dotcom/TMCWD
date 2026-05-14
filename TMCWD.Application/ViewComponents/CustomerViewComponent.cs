using Microsoft.AspNetCore.Mvc;
using TMCWD.CustomerSupport;

namespace TMCWD.Application.ViewComponents
{
    public class CustomerViewComponent : ViewComponent
    {
        private readonly HttpClient _client;
        private readonly CustomerTransaction _customerTransaction;

        public CustomerViewComponent(IHttpClientFactory factory, CustomerTransaction customerTransaction)
        {
            _client = factory.CreateClient("TmcWdApi");
            _customerTransaction = customerTransaction;
            _customerTransaction.SetClient(_client);
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var customers = await _customerTransaction.GetCustomers();
            return View(customers);
        }
    }
}
