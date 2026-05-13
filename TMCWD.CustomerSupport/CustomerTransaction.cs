using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using TMCWD.Model.CustomerSupport;
using TMCWD.Utility.Generic;
using TMCWD.Model.Interfaces;
using System.Text.Json;

namespace TMCWD.CustomerSupport
{
    public class CustomerTransaction : TransactionBase
    {

        #region fields

        private const string _serviceRouteUrl = "api/Customer/";
        private const string _saveUpdateUrl = $"{_serviceRouteUrl}SaveUpdate";
        private const string _getByIdUrl = $"{_serviceRouteUrl}GetById";
        private const string _getByNameUrl = $"{_serviceRouteUrl}GetByName";
        private const string _getCustomersUrl = $"{_serviceRouteUrl}GetCustomers";

        private readonly HttpClient _client;

        #endregion

        #region constructors
        public CustomerTransaction(HttpClient client) { _client = client; }

        #endregion

        #region public methods

        public Customer ConvertJsonToCustomer(string json)
        {
            var serializeOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<Customer>(json, serializeOptions) ?? new Customer();
        }

        public List<Customer> ConvertJsonToCustomers(string json)
        {
            var serializeOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<Customer>>(json, serializeOptions) ?? new List<Customer>();
        }

        public async Task<Customer> SaveUpdate(int userId, Customer customer)
        {
            var content = JsonContent.Create(customer);

            var response = await _client.PostAsync($"api/Customer/SaveUpdate/{userId}", content);

            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            return this.ConvertJsonToCustomer(data);
        }

        public async Task<Customer?> Get(int id)
        {
            var response = await _client.GetAsync($"api/Customer/Get/{id}");
            var data = await response.Content.ReadAsStringAsync();
            if(!response.IsSuccessStatusCode) return null;
            return this.ConvertJsonToCustomer(data);
        }

        public async Task<List<Customer>?> GetByName(string firstname, string lastname)
        {
            var response = await _client.GetAsync($"api/Customer/GetByName/{firstname}/{lastname}");
            var data = await response.Content.ReadAsStringAsync();
            if(!response.IsSuccessStatusCode) return null;
            return this.ConvertJsonToCustomers(data);
        }

        public async Task<List<Customer>?> GetCustomers()
        {
            var response = await _client.GetAsync("api/Customer/GetCustomers");
            var data = await response.Content.ReadAsStringAsync();
            if(!response.IsSuccessStatusCode) return null;
            return this.ConvertJsonToCustomers(data);
        }

        #endregion

    }
}
