using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TMCWD.Model.CustomerSupport;
using TMCWD.Model.Interfaces;
using TMCWD.Utility.Generic;

namespace TMCWD.CustomerSupport
{
    public class InspectionReportTransaction : TransactionBase
    {

        #region fields
        //private const string _serviceRouteUrl = "api/InspectionReport/";
        //private const string _saveUpdateUrl = $"{_serviceRouteUrl}SaveUpdate";
        //private const string _getByIdUrl = $"{_serviceRouteUrl}GetById";
        //private const string _getByRequestIdUrl = $"{_serviceRouteUrl}GetByRequestId";

        private readonly HttpClient _client;
        #endregion

        #region constructors
        public InspectionReportTransaction(HttpClient client) { _client = client; }
        #endregion

        #region public methods

        public InspectionReport ConvertJsonToInspectionReport(string json)
        {
            var serializeOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<InspectionReport>(json, serializeOptions) ?? new InspectionReport();
        }

        public List<InspectionReport> ConvertJsonToInspectionReports(string json)
        {
            var serializeOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<InspectionReport>>(json, serializeOptions) ?? new List<InspectionReport>();
        }

        public async Task<InspectionReport> SaveUpdate(int userId, InspectionReport report)
        {
            StringBuilder sb = new();
            if (report == null) throw new Exception("Inspection report data is empty");
            if (report.RequestId <= 0) sb.AppendLine("Request is required before creating inspection report");
            if (String.IsNullOrEmpty(report.Details.Trim())) sb.AppendLine("Inspection report details is not provided");

            if (String.IsNullOrEmpty(sb.ToString().Trim())) throw new Exception(sb.ToString());

            var content = JsonContent.Create(report);

            var response = await _client.PostAsync($"api/InspectionReport/SaveUpdate/{userId}", content);
            var data = await response.Content.ReadAsStringAsync();
            if(!response.IsSuccessStatusCode) throw new Exception(data);

            return ConvertJsonToInspectionReport(data);
        }

        public async Task<InspectionReport> Get(int id)
        {

            if (id <= 0) throw new Exception("Inspection report id is required to get details.");

            var response = await _client.GetAsync($"api/InspectionReport/Get/{id}");
            var data = await response.Content.ReadAsStringAsync();
            if(!response.IsSuccessStatusCode) throw new Exception(data);
            
            return ConvertJsonToInspectionReport(data);
        }

        public async Task<List<InspectionReport>> GetByRequestId(int requestId)
        {
            if (requestId <= 0) throw new Exception("Request id is required to get inspection report");
            
            var response = await _client.GetAsync($"api/InspectionReport/GetByRequestId/{requestId}");
            var data = await response.Content.ReadAsStringAsync();
            if(!response.IsSuccessStatusCode) throw new Exception(data);

            return ConvertJsonToInspectionReports(data);
        }

        #endregion

        #region private method

        //private async Task<bool> SaveUpdateTask(InspectionReport report)
        //{
        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(this.BaseUrl);
        //            HttpContent content = JsonContent.Create(report);
        //            using(var response = await client.PostAsync(_saveUpdateUrl, content))
        //            {
        //                var data = await response.Content.ReadAsStringAsync();
        //                if (!response.IsSuccessStatusCode) throw new Exception(data);
        //                return data.ToLower().Trim() == "true";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
        //    }
        //    return false;
        //}

        //private async Task<InspectionReport> GetByIdTask(int id)
        //{
        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(this.BaseUrl);
        //            string url = QueryHelpers.AddQueryString(_getByIdUrl, "id", id.ToString());
        //            using(var response = await client.GetAsync(url))
        //            {
        //                var data = await response.Content.ReadAsStringAsync();
        //                if (!response.IsSuccessStatusCode) throw new Exception(data);
        //                var serialized = JsonSerializer.Deserialize<InspectionReport>(data);
        //                if (serialized != null) return serialized;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
        //    }

        //    return new InspectionReport();
        //}

        //private async Task<List<InspectionReport>> GetByRequestIdTask(int requestId)
        //{

        //    try
        //    {
        //        using (HttpClient client = new HttpClient())
        //        {
        //            client.BaseAddress = new Uri(this.BaseUrl);
        //            string url = QueryHelpers.AddQueryString(_getByRequestIdUrl, "requestId", requestId.ToString());
        //            using(var response = await client.GetAsync(url))
        //            {
        //                var data = await response.Content.ReadAsStringAsync();
        //                if (!response.IsSuccessStatusCode) throw new Exception(data);
        //                var serialized = JsonSerializer.Deserialize<List<InspectionReport>>(data);
        //                if (serialized != null) return serialized;
        //            }
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
        //    }

        //    return new List<InspectionReport>();
        //}

        #endregion

    }
}
