using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;
using System.Text.Json;
using TMCWD.Model.Administrator;
using TMCWD.Utility.Generic;
using TMCWD.Model.Interfaces;

namespace TMCWD.Administration
{
    public class InspectionTypeTransaction : TransactionBase
    {

        #region fields

        private HttpClient _client = new();

        #endregion

        #region constructors

        public InspectionTypeTransaction() { }

        #endregion

        #region public methods
        public void SetClient(HttpClient client)
        {
            _client = client;
        }

        public InspectionType ConvertJsonToInspectionType(string json)
        {
            var serializeOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<InspectionType>(json, serializeOptions) ?? new InspectionType();
        }

        public List<InspectionType> ConvertJsonToInspectionTypes(string json)
        {
            var serializeOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<InspectionType>>(json, serializeOptions) ?? new List<InspectionType>();
        }

        public async Task<List<InspectionType>> GetTypes()
        {
            var response = await _client.GetAsync("api/InspectionType/GetTypes");
            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            return this.ConvertJsonToInspectionTypes(data);
        }

        public async Task<InspectionType> SaveUpdate(int userId, InspectionType type)
        {

            var content = JsonContent.Create(type);
            var response = await _client.PostAsync($"api/InspectionType/SaveUpdate/{userId}", content);

            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;

            return this.ConvertJsonToInspectionType(data);
        }

        public async Task<InspectionType> Get(int id)
        {
            var response = await _client.GetAsync($"api/InspectionType/Get/{id}");
            var data = await response.Content.ReadAsStringAsync();
            if(!response.IsSuccessStatusCode) return null;

            return this.ConvertJsonToInspectionType(data);
        }

        public async Task<InspectionType> GetNewRequestType()
        {
            var response = await _client.GetAsync($"api/InspectionType/GetNewRequestType");
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) return null;
            return this.ConvertJsonToInspectionType(data);
        }

        #endregion

    }
}
