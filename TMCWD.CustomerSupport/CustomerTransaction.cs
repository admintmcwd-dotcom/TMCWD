using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using TMCWD.Model.CustomerSupport;
using TMCWD.Utility.Generic;
using TMCWD.Model.Interfaces;

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

        #endregion

        #region constructors
        public CustomerTransaction() { }

        #endregion

        #region public methods

        public bool SaveUpdate(Customer customer)
        {
            bool isSuccess = false;

            try
            {

                if (customer == null) throw new Exception("Customer data was not supplied");
                if (String.IsNullOrEmpty(customer.Firstname.Trim())) throw new Exception("Customer firstname is required");
                if (String.IsNullOrEmpty(customer.Lastname.Trim())) throw new Exception("Customer lastname is required");
                if (String.IsNullOrEmpty(customer.PhoneNumber.Trim())) throw new Exception("Customer phone number is required");
                if (String.IsNullOrEmpty(customer.Email.Trim())) throw new Exception("Customer email is required");
                if (customer.Id > 0) customer.DateUpdated = DateTime.Now;
                else customer.DateCreated = DateTime.Now;

                if (customer == null) return isSuccess;
                isSuccess = Task.Run(() => SaveUpdateTask(customer)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, $"Error in SaveUpdate: {ex.Message}");
            }

            return isSuccess;
        }

        public Customer? GetById(int id)
        {
            Customer? customer = new();
            try
            {
                if (id <= 0) throw new Exception("Invalid customer ID supplied.");
                customer = Task.Run(() => GetByIdTask(id)).GetAwaiter().GetResult();
                if(customer == null) throw new Exception($"Customer with ID {id} not found.");
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, $"Error in GetById: {ex.Message}");
            }
            return customer;
        }

        public Customer? GetByName(string firstname, string lastname)
        {
            Customer? customer = new();
            try
            {
                if (String.IsNullOrEmpty(firstname.Trim())) throw new Exception("Customer firstname is required");
                if (String.IsNullOrEmpty(lastname.Trim())) throw new Exception("Customer lastname is required");
                customer = Task.Run(() => GetByNameTask(firstname, lastname)).GetAwaiter().GetResult();
                if (customer == null) throw new Exception($"Customer with name {firstname} {lastname} not found.");
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, $"Error in GetByName: {ex.Message}");
            }
            return customer;
        }

        public List<Customer>? GetCustomers()
        {
            List<Customer>? customers = new();
            try
            {
                customers = Task.Run(() => GetCustomersTask()).GetAwaiter().GetResult();
                if (customers == null || customers.Count == 0) throw new Exception("No customers found.");
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, $"Error in GetCustomers: {ex.Message}");
            }
            return customers;
        }

        #endregion

        #region private methods

        private async Task<bool> SaveUpdateTask(Customer customer)
        {
            bool isSuccess = false;

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    Dictionary<string, string> queryParams = new()
                    {
                        { "Id", customer.Id.ToString() },
                        { "Firstname", customer.Firstname },
                        { "Lastname", customer.Lastname },
                        { "Middlename", customer.Middlename },
                        { "PhoneNumber", customer.PhoneNumber },
                        { "Email", customer.Email },
                        { "DateCreated", customer.DateCreated.ToString("o") },
                        { "DateUpdated", customer.DateUpdated.ToString("o") },
                        { "IsActive", customer.IsActive.ToString() }
                    };
                    HttpContent content = JsonContent.Create(customer);
                    using(var response = await client.PostAsync(_saveUpdateUrl, content))
                    {
                        if(!response.IsSuccessStatusCode)
                        {
                            Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, $"Failed to save/update customer. Status Code: {response.StatusCode}");
                        }
                        else
                        {
                            isSuccess = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, $"Error saving/updating customer: {ex.Message}");
            }

            return isSuccess;
        }

        private async Task<Customer?> GetByIdTask(int id)
        {
            Customer? customer = new();

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByIdUrl, "id", id.ToString());
                    using(var response = await client.GetAsync(url))
                    {
                        if(!response.IsSuccessStatusCode)
                        {
                            Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, $"Failed to retrieve customer by ID. Status Code: {response.StatusCode}");
                        }
                        else
                        {
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            customer = System.Text.Json.JsonSerializer.Deserialize<Customer>(jsonResponse);
                            if(customer == null) throw new Exception($"Customer with ID {id} not found.");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, $"Error retrieving customer by ID: {ex.Message}");
            }

            return customer;
        }

        private async Task<Customer?> GetByNameTask(string firstname, string lastname)
        {
            Customer? customer = new();

            try
            {

                using (HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByNameUrl, new Dictionary<string, string?>
                    {
                        { "firstname", firstname },
                        { "lastname", lastname }
                    });
                    using (var response = await client.GetAsync(url))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, $"Failed to retrieve customer by name. Status Code: {response.StatusCode}");
                        }
                        else
                        {
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            customer = System.Text.Json.JsonSerializer.Deserialize<Customer>(jsonResponse);
                            if (customer == null) throw new Exception($"Customer with name {firstname} {lastname} not found.");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, $"Error retrieving customer by name: {ex.Message}");
            }

            return customer;
        }

        private async Task<List<Customer>?> GetCustomersTask()
        {
            List<Customer>? customers = new();

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    using (var response = await client.GetAsync(_getCustomersUrl))
                    {
                        if (!response.IsSuccessStatusCode)
                        {
                            Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, $"Failed to retrieve customers. Status Code: {response.StatusCode}");
                        }
                        else
                        {
                            string jsonResponse = await response.Content.ReadAsStringAsync();
                            customers = System.Text.Json.JsonSerializer.Deserialize<List<Customer>>(jsonResponse);
                            if (customers == null) throw new Exception("No customers found.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, $"Error retrieving customers: {ex.Message}");
            }

            return customers;
        }

        #endregion

    }
}
