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
        #endregion

        #region constructors
        public InventoryTransaction() { }
        #endregion

        #region public methods

        public bool SaveUpdate(Inventory inventory)
        {

            try
            {
                if (inventory == null) throw new Exception("Inventory data is null");
                if (inventory.Division <= 0) throw new Exception("Please specify which division");
                if (String.IsNullOrEmpty(inventory.Unit.Trim())) throw new Exception("Please specify the item unit");
                if (String.IsNullOrEmpty(inventory.Name.Trim())) throw new Exception("Please specify the item name");
                if (inventory.UnitCost <= 0) throw new Exception("Please specify the unit cost");

                return Task.Run(() => SaveUpdateTask(inventory)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Engineering, ErrorType.Error, ex.Message);
            }

            return false;
        }

        public Inventory GetById(int id)
        {
            Inventory inventory = new();
            try
            {
                if (id <= 0) throw new Exception("Id is required to get inventory item");

                inventory = Task.Run(() => GetByIdTask(id)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Engineering, ErrorType.Error, ex.Message);
            }
            return inventory;
        }

        public List<Inventory> GetAll()
        {
            List<Inventory> inventories = new();

            try
            {
                inventories = Task.Run(() => GetAllTask()).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Engineering, ErrorType.Error, ex.Message);
            }

            return inventories;
        }

        public List<Inventory> GetByName(string name)
        {
            List<Inventory> inventories = new();

            try
            {
                if (name.Length <= 4) return new List<Inventory>();
                inventories = Task.Run(() => GetByNameTask(name)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Engineering, ErrorType.Error, ex.Message);
            }

            return inventories;
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
                        var serialized = JsonSerializer.Deserialize<Inventory>(data);
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
                        var serialized = JsonSerializer.Deserialize<List<Inventory>>(data);
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
                        var serialized = JsonSerializer.Deserialize<List<Inventory>>(data);
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
