using Microsoft.AspNetCore.Mvc;
using TMCWD.CustomerSupport;
using TMCWD.Model.CustomerSupport;

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
            Task<Account> getAccount = _accountTransaction.Get(accountId);
            await Task.WhenAll(getAccount);
            var account = getAccount.Result;
            if (account == null) return View("Default", "");
            return View("Default", $"{account.AccountNumber ?? string.Empty}");
        }

    }
}
