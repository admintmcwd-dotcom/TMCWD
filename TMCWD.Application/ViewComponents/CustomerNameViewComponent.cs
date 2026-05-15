using Microsoft.AspNetCore.Mvc;
using TMCWD.Application.Models;
using TMCWD.CustomerSupport;

namespace TMCWD.Application.ViewComponents
{
    public class CustomerNameViewComponent : ViewComponent
    {
        private readonly HttpClient _client;
        private readonly CustomerTransaction _customerTransaction;

        public CustomerNameViewComponent(IHttpClientFactory factory, CustomerTransaction customerTransaction)
        {
            _client = factory.CreateClient("TmcWdApi");
            _customerTransaction = customerTransaction;
            _customerTransaction.SetClient(_client);
        }

        public async Task<IViewComponentResult> InvokeAsync(int customerId, string pId = "", string pClass = "")
        {
            CustomerComponentViewModel model = new();
            model.ParagraphClass = pClass;
            model.ParagraphId = pId;
            if (customerId <= 0) model.Customer = new();
            else
            {
                model.Customer = await _customerTransaction.Get(customerId);
            }
            return View("Default", model);
        }
    }
}
