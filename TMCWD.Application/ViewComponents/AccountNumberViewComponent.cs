using Microsoft.AspNetCore.Mvc;
using TMCWD.CustomerSupport;

namespace TMCWD.Application.ViewComponents
{
    public class AccountNumberViewComponent : ViewComponent
    {

        private readonly HttpClient _client;
        private readonly AccountTransaction _accountTransaction;

        public AccountNumberViewComponent(IHttpClientFactory factory, AccountTransaction accountTransaction)
        {
            _client = factory.CreateClient("TmcWdApi");
            _accountTransaction = accountTransaction;
            _accountTransaction.SetClient(_client);
        }

        public async Task<IViewComponentResult> InvokeAsync(int accountId)
        {
            var account = await _accountTransaction.Get(accountId);
            return View("Default", $"{account.AccountNumber}");
        }

    }
}
