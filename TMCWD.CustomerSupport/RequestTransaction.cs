using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TMCWD.Model.CustomerSupport;
using TMCWD.Model.Interfaces;
using TMCWD.Utility.Generic;

namespace TMCWD.CustomerSupport
{
    public class RequestTransaction : TransactionBase
    {

        #region fields

        private const string _requestServiceRouteUrl = "api/Request/";
        private const string _requestDetailServiceRouteUrl = "api/RequestDetail/";
        private const string _saveUpdateUrl = $"{_requestServiceRouteUrl}SaveUpdate";
        private const string _getByIdUrl = $"{_requestServiceRouteUrl}GetById";
        private const string _getRequestsUrl = $"{_requestServiceRouteUrl}GetRequests";
        private const string _getByUserIdUrl = $"{_requestServiceRouteUrl}GetByUserId";
        private const string _getByCustomerIdUrl = $"{_requestServiceRouteUrl}GetByCustomerId";
        private const string _saveUpdateRequestDetailByRequestIdUrl = $"{_requestDetailServiceRouteUrl}SaveUpdate";
        private const string _saveMultipeRequestDetailUrl = $"{_requestDetailServiceRouteUrl}SaveMultiple";
        private const string _getRequestDetailByRequestId = $"{_requestDetailServiceRouteUrl}GetDetailsByRequestId";

        #endregion

        #region constructors

        public RequestTransaction() { }

        #endregion

        #region public methods

        public bool SaveUpdate(Request request)
        {
            try
            {

                if (request == null) throw new Exception("Request data has not been supplied to create or update a request");
                if(request.AccountId <= 0) throw new Exception("Creating or updating request requires account");
                if (request.CustomerId <= 0) throw new Exception("Creating or updating request requires customer with account");
                if (request.Id > 0) request.DateUpdated = DateTime.Now;
                else request.DateCreated = DateTime.Now;

                return Task.Run(() => SaveUpdateRequestTask(request)).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return false;
        }

        public Request GetById(int id)
        {
            try
            {
                if (id <= 0) throw new Exception("Getting request by id requires request id");
                return Task.Run(() => GetRequestByIdTask(id)).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return new Request();
        }

        public List<Request> GetRequests()
        {

            try
            {
                return Task.Run(() => GetRequestsTask()).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return new List<Request>();
        }

        public List<Request> GetByUserId(int userId)
        {

            try
            {
                if (userId <= 0) throw new Exception("Fetching request by user requires the user id");

                return Task.Run(() => GetRequestsByUserIdTask(userId)).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return new List<Request>();
        }

        public List<Request> GetByCustomerId(int customerId)
        {

            try
            {
                if (customerId <= 0) throw new Exception("Fetching customer requests requires customer id");
                return Task.Run(() => GetRequestsByCustomerIdTask(customerId)).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return new List<Request>();
        }

        public bool SaveUpdateRequestDetail(RequestDetail detail)
        {

            try
            {
                if (detail == null) throw new Exception("Request details is empty");
                if (string.IsNullOrEmpty(detail.InspectionTypeDetail.Trim())) throw new Exception("Inspection detail is not specified");
                if (detail.RequestTypeId <= 0) throw new Exception("Select at least one inspection type to be performed");
                if (detail.RequestId <= 0) throw new Exception("Request for detail is not found");

                return Task.Run(() => SaveUpdateRequestDetail(detail)).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return false;
        }

        public bool SaveMulipleRequestDetail(List<RequestDetail> details)
        {

            try
            {
                if (details == null || !details.Any()) throw new Exception("Request details is empty");

                var emptyDetailType = details.Where(x => string.IsNullOrEmpty(x.InspectionTypeDetail.Trim()));
                var noInspectionType = details.Where(x => x.RequestTypeId <= 0);
                var noRequestId = details.Where(x => x.RequestId <= 0);

                if (emptyDetailType == null || !emptyDetailType.Any()) throw new Exception("One of the request detail is missing the required inspection type detail");
                if (noInspectionType == null || !noInspectionType.Any()) throw new Exception("One of the request detail is missing the required inspection type");
                if (noRequestId == null || !noRequestId.Any()) throw new Exception("One of the request detail is missing the request");

                return Task.Run(() => SaveMulipleRequestDetail(details)).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return false;
        }


        #endregion

        #region private methods

        private async Task<bool> SaveUpdateRequestTask(Request request)
        {

            try
            {
                using (HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    HttpContent content = JsonContent.Create(request);
                    using(var response = await client.PostAsync(_saveUpdateUrl, content))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        return data.ToLower() == "true";
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return false;
        }

        private async Task<Request> GetRequestByIdTask(int id)
        {

            try
            {
                using (HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByIdUrl, "id", id.ToString());
                    using (var respons = await client.GetAsync(url))
                    {
                        var data = await respons.Content.ReadAsStringAsync();
                        if (!respons.IsSuccessStatusCode) throw new Exception(data);
                        var serialized = JsonSerializer.Deserialize<Request>(data);
                        if (serialized != null) return serialized;
                    }
                }
            }
            catch(Exception ex) 
            { 
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return new Request();
        }

        private async Task<List<Request>> GetRequestsTask()
        {

            try
            {
                using (HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    using (var response = await client.GetAsync(_getRequestsUrl))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        var serialized = JsonSerializer.Deserialize<List<Request>>(data);
                        if(serialized != null) return serialized;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return new List<Request>();
        }

        private async Task<List<Request>> GetRequestsByUserIdTask(int userId)
        {

            try
            {
                using (HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByUserIdUrl, "userId", userId.ToString());
                    using(var response = await client.GetAsync(_getByUserIdUrl))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        var serialized = JsonSerializer.Deserialize<List<Request>>(data);
                        if(serialized != null) return serialized;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return new List<Request>();
        }

        private async Task<List<Request>> GetRequestsByCustomerIdTask(int customerId)
        {

            try
            {
                using(HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByCustomerIdUrl, "customerId", customerId.ToString());
                    using(var response = await client.GetAsync(url))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        var serialized = JsonSerializer.Deserialize<List<Request>>(data);
                        if(serialized != null) return serialized;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return new List<Request>();
        }

        private async Task<bool> SaveUpdateRequestDetailTask(RequestDetail detail)
        {

            try
            {
                using (HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    HttpContent content = JsonContent.Create(detail);
                    using(var response = await client.PostAsync(_saveUpdateRequestDetailByRequestIdUrl, content))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        return data.ToLower() == "true";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return false;
        }

        private async Task<bool> SaveMultipleRequestDetailTask(List<RequestDetail> details)
        {

            try
            {
                using (HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    HttpContent content = JsonContent.Create(details);
                    using(var response = await client.PostAsync(_saveMultipeRequestDetailUrl, content))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        return data.ToLower() == "true";
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return false;
        }

        private async Task<List<RequestDetail>> GetRequestDetailsByRequestIdTask(int requestId)
        {
            try
            {
                using (HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getRequestDetailByRequestId, "requestId", requestId.ToString()); 
                    using(var response = await client.GetAsync(url))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        var serialized = JsonSerializer.Deserialize<List<RequestDetail>>(data);
                        if (serialized != null) return serialized;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return new List<RequestDetail>();
        }

        #endregion

    }
}
