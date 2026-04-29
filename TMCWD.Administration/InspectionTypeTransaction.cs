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

        private const string _serviceRouteUrl = "api/InspectionType/";
        private const string _getTypesUrl = $"{_serviceRouteUrl}GetTypes";
        private const string _getByIdUrl = $"{_serviceRouteUrl}GetById";
        private const string _saveUpdateUrl = $"{_serviceRouteUrl}SaveUpdate";

        #endregion

        #region constructors

        public InspectionTypeTransaction()
        {
        }

        #endregion

        #region public methods

        public List<InspectionType>? GetIncidentTypes()
        {
            try
            {
                var res = Task.Run(() => GetInspectionTypesTask()).GetAwaiter().GetResult();
                if (!res.Any()) throw new Exception("No incident type found");
                return res;
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return null;
        }

        public bool SaveUpdateInspectionType(InspectionType type)
        {
            bool isSuccess = false;

            try
            {
                if (String.IsNullOrEmpty(type.Name.Trim())) throw new Exception("Incident type name is required");
                if (type.Id > 0) type.DateUpdated = DateTime.Now;
                else type.DateCreated = DateTime.Now;

                isSuccess = Task.Run(() => SaveUpdateInspectionTypeTask(type)).GetAwaiter().GetResult();

            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return isSuccess;
        }

        public InspectionType? GetInspectionTypeById(int id)
        {
            InspectionType? type = new();

            try
            {
                if (id <= 0) throw new Exception("Inspection type id is required");
                type = Task.Run(() => GetInspectionTypeByIdTask(id)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return type;
        }

        #endregion

        #region private methods

        private async Task<List<InspectionType>?> GetInspectionTypesTask()
        {
            using (HttpClient client = new())
            {
                client.BaseAddress = new Uri(this.BaseUrl);
                using (var response = await client.GetAsync(_getTypesUrl))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();

                        return JsonSerializer.Deserialize<List<InspectionType>>(data);
                    }
                    else
                    {
                        throw new Exception($"Failed to retrieve incident types. Status code: {response.StatusCode}");
                    }
                }
            }
        }

        private async Task<bool> SaveUpdateInspectionTypeTask(InspectionType type)
        {
            bool isSuccess = false;

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    HttpContent content = JsonContent.Create(type);
                    using(var response = await client.PostAsync(_saveUpdateUrl, content))
                    {
                        if(response.IsSuccessStatusCode)
                        {
                            isSuccess = true;
                        }
                        else
                        {
                            throw new Exception($"Failed to save/update incident. Status code: {response.StatusCode}");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return isSuccess;
        }

        private async Task<InspectionType?> GetInspectionTypeByIdTask(int id)
        {
            InspectionType type = new();

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    using (var response = await client.GetAsync(QueryHelpers.AddQueryString(_getByIdUrl, new Dictionary<string, string?>
                    {
                        { "id", id.ToString() }
                    })))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        return JsonSerializer.Deserialize<InspectionType>(data);
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.Administration, ErrorType.Error, ex.Message);
            }

            return null;
        }

        #endregion

    }
}
