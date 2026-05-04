using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TMCWD.Model.CustomerSupport;
using TMCWD.Model.Interfaces;
using TMCWD.Utility.Generic;

namespace TMCWD.CustomerSupport
{
    public class AccountTransaction : TransactionBase
    {

        #region fields

        private const string _serviceRoute = "api/Account/";
        private const string _saveUpdateUrl = $"{_serviceRoute}SaveUpdate";
        private const string _getByIdUrl = $"{_serviceRoute}GetById";
        private const string _getByAccountNumberUrl = $"{_serviceRoute}GetByAccountNumber";
        private const string _getByCustomerIdUrl = $"{_serviceRoute}GetByCustomerId";
        private const string _getByMeterNumberUrl = $"{_serviceRoute}GetByMeterNumber";

        #endregion

        #region constructors

        public AccountTransaction() { }

        #endregion

        #region public methods

        public bool SaveUpdate(Account account)
        {

            try
            {
                if (account == null) throw new Exception("Required account fields are not supplied");
                if (String.IsNullOrEmpty(account.AccountNumber.Trim())) throw new Exception("Äccount number is required");
                if (String.IsNullOrEmpty(account.MeterNumber.Trim())) throw new Exception("Meter number is required");
                if (account.CustomerId <= 0) throw new Exception("No customer has been selected for this account");
                if (String.IsNullOrEmpty(account.Address.Trim())) throw new Exception("Account address is required for account creation");
                return Task.Run(() => SaveUpdateAccountTask(account)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return false;
        }

        public Account GetById(int id)
        {

            try
            {
                if (id <= 0) throw new Exception("Äccount id is not specified");
                return Task.Run(() => GetAccountByIdTask(id)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return new Account();
        }

        public Account GetByAccountNumber(string accountNumber)
        {
            try
            {
                if (String.IsNullOrEmpty(accountNumber.Trim())) throw new Exception("Account number is required to get account details");
                return Task.Run(() => GetAccountByAccountNumberTask(accountNumber)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }
            return new Account();
        }

        public List<Account> GetByCustomerId(int id)
        {

            try
            {
                if (id <= 0) throw new Exception("Customer id is required to get accounts bound to customer");
                return Task.Run(() => GetAccountByCustomerIdTask(id)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return new List<Account>();
        }

        public Account GetByMeterNumber(string meterNumber)
        {



            return new Account();
        }

        #endregion

        #region private methods

        private async Task<bool> SaveUpdateAccountTask(Account account)
        {
            bool isSuccess = false;

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    HttpContent content = JsonContent.Create(account);
                    using (var response = await client.PostAsync(_saveUpdateUrl, content))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        isSuccess = data.ToLower() == "true";
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return isSuccess;
        }

        private async Task<Account> GetAccountByIdTask(int id)
        {
            Account account = new();

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByIdUrl, "id", id.ToString());
                    using(var response = await client.GetAsync(url))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        var serialized = JsonSerializer.Deserialize<Account>(data);
                        if (serialized != null) account = serialized;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return account;
        }

        private async Task<Account> GetAccountByAccountNumberTask(string accountNumber)
        {
            Account account = new();

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByAccountNumberUrl, "accountNumber", accountNumber);
                    using(var response = await client.GetAsync(url))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        var serialized = JsonSerializer.Deserialize<Account>(data);
                        if(serialized != null) account = serialized;
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return account;
        }

        private async Task<List<Account>> GetAccountByCustomerIdTask(int id)
        {
            List<Account> accounts = new();

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByCustomerIdUrl, "customerId", id.ToString());
                    using (var response = await client.GetAsync(url))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        var serialized = JsonSerializer.Deserialize<List<Account>>(data);
                        if (serialized != null) accounts = serialized;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return accounts;
        }

        private async Task<Account> GetAccountByMeterNumberTask(string meterNumber)
        {
            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByMeterNumberUrl, "meterNumber", meterNumber);
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }
            return new Account();
        }

        #endregion

    }
}
