using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TMCWD.Model.CustomerSupport;

namespace TMCWD.CustomerSupport
{
    public class JobOrderTransaction
    {
        private HttpClient _client = new();
        public JobOrderTransaction() { }

        public void SetClient(HttpClient client) { _client = client; }

        public JobOrder ConvertJsonToJobOrder(string json)
        {
            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<JobOrder>(json, serializerOptions) ?? new();
        }
        
        public List<JobOrder> ConvertJsonToJobOrders(string json)
        {
            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<JobOrder>>(json, serializerOptions) ?? new();
        }

        public async Task<JobOrder> Get(int id, int requestId)
        {
            var response = await _client.GetAsync($"api/{requestId}/JobOrder/Get/{id}");
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return ConvertJsonToJobOrder(data);
        }

        public async Task<List<JobOrder>> GetAll(int requestId)
        {
            var response = await _client.GetAsync($"api/{requestId}/JobOrder/GetAll");
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return ConvertJsonToJobOrders(data);
        }

        public async Task<JobOrder> SaveUpdate(int userId, int requestId, JobOrder jobOrder)
        {
            var content = JsonContent.Create(jobOrder);
            var response = await _client.PostAsync($"api/{requestId}/JobOrder/SaveUpdate/{userId}", content);
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return ConvertJsonToJobOrder(data);
        }

    }
}
