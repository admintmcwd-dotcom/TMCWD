using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TMCWD.Model.Engineering;
using TMCWD.Model.Interfaces;
using TMCWD.Utility.Generic;

namespace TMCWD.Engineering
{
    public class InventoryTransaction : TransactionBase
    {

        #region fields
        private const string _serviceRouteUrl = "api/Inventory/";
        private const string _saveUpdateUrl = $"{_serviceRouteUrl}SaveUpdate";
        private const string _getByIdUrl = $"{_serviceRouteUrl}GetById";
        private const string _getAllUrl = $"{_serviceRouteUrl}GetAll";
        private const string _getByNameUrl = $"{_serviceRouteUrl}GetByName";

        private HttpClient _client = new();

        #endregion

        #region constructors
        public InventoryTransaction() { }
        #endregion

        #region public methods

        public void SetClient(HttpClient client)
        {
            _client = client;
        }

        public Inventory ConvertJsonToInventory(string json)
        {
            var serializeOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<Inventory>(json, serializeOptions) ?? new Inventory();
        }

        public List<Inventory> ConvertJsonToInventoryItems(string json)
        {
            var serializeOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<Inventory>>(json, serializeOptions) ?? new List<Inventory>();
        }

        public async Task<Inventory> SaveUpdate(int userId, Inventory inventory)
        {

            if (inventory == null) throw new Exception("Inventory data is null");

            StringBuilder sb = new();

            if (inventory.Division <= 0) sb.AppendLine("Please specify which division");
            if (String.IsNullOrEmpty(inventory.UOM.Trim())) sb.AppendLine("Please specify the item unit");
            if (String.IsNullOrEmpty(inventory.Name.Trim())) sb.AppendLine("Please specify the item name");
            if (inventory.UnitCost <= 0) sb.AppendLine("Please specify the unit cost");

            if(!String.IsNullOrEmpty(sb.ToString().Trim())) throw new Exception(sb.ToString());
             
            var content = JsonContent.Create(inventory);

            var response = await _client.PostAsync($"api/Inventory/SaveUpdate/{userId}", content);

            var data = await response.Content.ReadAsStringAsync();

            if(!response.IsSuccessStatusCode) throw new Exception(data);

            return ConvertJsonToInventory(data);
        }

        public async Task<Inventory> Get(int id)
        {

            if (id <= 0) throw new Exception("Id is required to get inventory item");

            var response = await _client.GetAsync($"api/Inventory/Get/{id}");

            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            return ConvertJsonToInventory(data);
        }

        public async Task<List<Inventory>> GetAll()
        {

            var response = await _client.GetAsync($"api/Inventory/GetAll");
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return ConvertJsonToInventoryItems(data);
        }

        public async Task<List<Inventory>> GetByName(string name)
        {

            var response = await _client.GetAsync($"api/Inventory/GetByName/{name}");
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return ConvertJsonToInventoryItems(data);
        }

        #endregion

        #region private methods

        private async Task<bool> SaveUpdateTask(Inventory inventory)
        {

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    HttpContent content = JsonContent.Create(inventory);
                    using(var response = await client.PostAsync(_saveUpdateUrl, content))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        return data.ToLower().Trim() == "true";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.Engineering, ErrorType.Error, ex.Message);
            }

            return false;
        }

        private async Task<Inventory> GetByIdTask(int id)
        {
            Inventory inventory = new();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByIdUrl, "id", id.ToString());
                    using(var response = await client.GetAsync(url))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode || data == null) throw new Exception(data);
                        var serializeOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                        var serialized = JsonSerializer.Deserialize<Inventory>(data, serializeOptions);
                        if(serialized != null) inventory = serialized;
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Engineering, ErrorType.Error, ex.Message);
            }

            return inventory;
        }

        private async Task<List<Inventory>> GetAllTask()
        {
            List<Inventory> inventories = new();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    using(var response = await client.GetAsync(_getAllUrl))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        var serializeOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                        var serialized = JsonSerializer.Deserialize<List<Inventory>>(data, serializeOptions);
                        if(serialized != null) inventories = serialized;
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Engineering, ErrorType.Error, ex.Message);
            }

            return inventories;
        }

        private async Task<List<Inventory>> GetByNameTask(string name)
        {
            List<Inventory> inventories = new();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByNameUrl, "name", name);
                    using(var response = await client.GetAsync(url))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        var serializeOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
                        var serialized = JsonSerializer.Deserialize<List<Inventory>>(data, serializeOptions);
                        if (serialized != null) inventories = serialized;
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Engineering, ErrorType.Error, ex.Message);
            }

            return inventories;
        }

        #endregion

    }
}
