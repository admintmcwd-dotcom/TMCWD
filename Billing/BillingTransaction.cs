using System.Net.Http.Json;
using System.Text.Json;
using TMCWD.Services;

namespace TMCWD.Billing
{
    public class BillingTransaction
    {

        #region fields

        private readonly WebService _service;

        #endregion

        #region constructors

        public BillingTransaction(WebService service) { _service = service; }

        #endregion

        #region methods

        public TMCWD.Model.Billing.Billing ConvertJsonToBilling(string json)
        {
            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<TMCWD.Model.Billing.Billing>(json, serializerOptions) ?? new();
        }

        public List<TMCWD.Model.Billing.Billing> ConverJsonToBillings(string json)
        {
            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };  
            return JsonSerializer.Deserialize<List<TMCWD.Model.Billing.Billing>>(json, serializerOptions) ?? new();
        }

        public async Task<TMCWD.Model.Billing.Billing> Get(int id)
        {
            var response = await _service.Client.GetAsync($"api/Billing/Get/{id}");
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return ConvertJsonToBilling(data);
        }

        public async Task<List<TMCWD.Model.Billing.Billing>> GetAll()
        {
            var response = await _service.Client.GetAsync($"api/Billing/GetAll");
            var data = await response.Content.ReadAsStringAsync();
            return ConverJsonToBillings(data);
        }

        public async Task<TMCWD.Model.Billing.Billing> GetByReference(string reference)
        {
            var response = await _service.Client.GetAsync($"api/Billing/GetByRefence/{reference}");
            var data = await response.Content.ReadAsStringAsync();
            return ConvertJsonToBilling(data);
        }

        public async Task<TMCWD.Model.Billing.Billing> SaveUpdate(int userId, TMCWD.Model.Billing.Billing billing)
        {
            var content = JsonContent.Create(billing);
            var response = await _service.Client.PostAsync($"api/Billing/SaveUpdate/{userId}", content);
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return ConvertJsonToBilling(data);
        }

        #endregion

    }
}
