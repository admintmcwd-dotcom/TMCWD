using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TMCWD.Model.CustomerSupport;

namespace TMCWD.CustomerSupport
{
    public class MaterialTransaction
    {
        private HttpClient _client = new HttpClient();

        public MaterialTransaction() { }

        public void SetClient(HttpClient client) { _client = client; }

        public Material ConvertJsonToMaterial(string json)
        {
            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<Material>(json, serializerOptions) ?? new();
        }

        public List<Material> ConvertJsonToMaterials(string json)
        {
            var serializerOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<Material>>(json, serializerOptions) ?? new();
        }

        public async Task<Material> Get(int requestId, int id)
        {
            var response = await _client.GetAsync($"api/{requestId}/Material/{id}");

            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            return ConvertJsonToMaterial(data);
        }

        public async Task<List<Material>> GetAll() 
        {
            var response = await _client.GetAsync("api/0/Material/GetAll");

            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            return ConvertJsonToMaterials(data);
        }

        public async Task<List<Material>> GetByRequestId(int requestId)
        {
            var response = await _client.GetAsync($"api/{requestId}/Material/GetByRequestId");
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return ConvertJsonToMaterials(data);
        }

        public async Task<Material> SaveUpdate(int userId, int requestId, Material material)
        {
            var content = JsonContent.Create(material);

            var response = await _client.PostAsync($"api/{requestId}/Material/SaveUpdate/{userId}", content);

            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            return ConvertJsonToMaterial(data);
        }

        public async Task<Material> UpdateQuantityOrNewUnitCost(int userId, int requestId, int id, int quantity, float unitCost)
        {
            Material material = new()
            {
                Id = id,
                RequestedQuantity = quantity,
                NewUnitCost = unitCost
            };

            var content = JsonContent.Create(material);

            var response = await _client.PutAsync($"api/{requestId}/Material/UpdateQuantityOrNewUnitCost/{userId}", content);
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return ConvertJsonToMaterial(data);
        }
    }

}
