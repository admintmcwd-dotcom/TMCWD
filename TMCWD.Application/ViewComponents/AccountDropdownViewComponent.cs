using Microsoft.AspNetCore.Mvc;
using TMCWD.CustomerSupport;

namespace TMCWD.Application.ViewComponents
{
    public class AccountDropdownViewComponent : ViewComponent
    {

        private readonly HttpClient _client;
        private readonly AccountTransaction _accountTransaction;

        public AccountDropdownViewComponent(IHttpClientFactory factory, AccountTransaction accountTransaction)
        {
            _client = factory.CreateClient("TmcWdApi");
            _accountTransaction = accountTransaction;
            _accountTransaction.SetClient(_client);
        }

        public async Task<IViewComponentResult> InvokeAsync(int customerId)
        {
            var accounts = await _accountTransaction.GetByCustomerId(customerId);
            return View("Default", accounts);
        }

    }
}
