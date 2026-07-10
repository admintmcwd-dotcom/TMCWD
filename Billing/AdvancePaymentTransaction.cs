using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TMCWD.Model.Billing;
using TMCWD.Services;

namespace TMCWD.Billing
{
    public class AdvancePaymentTransaction
    {

        #region fields

        private readonly WebService _service;

        #endregion


        #region constructors

        public AdvancePaymentTransaction(WebService service)
        {
            _service = service;
        }

        #endregion

        #region methods

        public AdvancePayment ConvertJsonToAdvancePayment(string json)
        {
            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<AdvancePayment>(json, serializerOptions) ?? new();
        }

        public List<AdvancePayment> ConvertJsonToAdvancePayments(string json)
        {
            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<AdvancePayment>>(json, serializerOptions) ?? new();
        }

        public async Task<AdvancePayment> Get(int id)
        {
            var response = await _service.Client.GetAsync($"api/AdvancePayment/Get/{id}");
            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;
            return ConvertJsonToAdvancePayment(data);
        }

        public async Task<List<AdvancePayment>> GetAll()
        {
            var response = await _service.Client.GetAsync("api/AdvancePayment/GetAll");
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return ConvertJsonToAdvancePayments(data);
        }

        public async Task<AdvancePayment> GetActiveByAccount(int accountId)
        {
            var response = await _service.Client.GetAsync($"api/AdvancePayment/GetActiveByAccount/{accountId}");
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return ConvertJsonToAdvancePayment(data);
        }

        public async Task<List<AdvancePayment>> GetByAccount(int accountId)
        {
            var response = await _service.Client.GetAsync($"api/GetByAccount/{accountId}");
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return ConvertJsonToAdvancePayments(data);
        }

        public async Task<AdvancePayment> SaveUpdate(int userId, AdvancePayment advancePayment)
        {
            var content = JsonContent.Create(advancePayment);
            var response = await _service.Client.PostAsync($"SaveUpdate/{userId}", content);
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return ConvertJsonToAdvancePayment(data);
        }

        #endregion

    }
}
