using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using TMCWD.Model.CustomerSupport;
using TMCWD.Model.CustomerSupport.Interfaces;
using TMCWD.Model.Interfaces;
using TMCWD.Utility.Generic;

namespace TMCWD.CustomerSupport
{
    public class RecommendationTransaction : TransactionBase
    {

        #region fields

        private const string _serviceRouteUrl = "api/Recommendation/";
        private const string _saveUpdateUrl = $"{_serviceRouteUrl}SaveUpdate";
        private const string _getByIdUrl = $"{_serviceRouteUrl}GetById";
        private const string _getByRequestId = $"{_serviceRouteUrl}GetByRequestId";

        #endregion

        #region constructors

        public RecommendationTransaction() { }

        #endregion

        #region public methods

        public bool SaveUpdate(Recommendation recommendation)
        {

            try
            {
                if (recommendation == null) throw new Exception("Recommendation is empty");
                if (String.IsNullOrEmpty(recommendation.Details.Trim())) throw new Exception("Recommendation detail is not provided");
                if (recommendation.RequestId <= 0) throw new Exception("No request is provided for this recommendation");

                return Task.Run(() => SaveUpdateTask(recommendation)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return false;
        }

        public Recommendation GetById(int id)
        {
            Recommendation recommendation = new();

            try
            {
                if (id <= 0) throw new Exception("Recommendation id is required");

                return Task.Run(() => GetByIdTask(id)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return recommendation;
        }

        public List<Recommendation> GetByRequestId(int requestId)
        {
            List<Recommendation> recommendations = new();

            try
            {
                if (requestId <= 0) throw new Exception("Request id is required to get recommendations");

                return Task.Run(() => GetByRequestIdTask(requestId)).GetAwaiter().GetResult();
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return recommendations;
        }

        #endregion

        #region private methods

        private async Task<bool> SaveUpdateTask(Recommendation recommendation)
        {

            try
            {
                using (HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    HttpContent content = JsonContent.Create(recommendation);
                    using (var response = await client.PostAsync(_saveUpdateUrl, content))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        return data.ToLower().Trim() == "true";
                    }
                }
            }
            catch(Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return false;
        }

        private async Task<Recommendation> GetByIdTask(int id)
        {
            Recommendation recommendation = new();
            try
            {
                using (HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByIdUrl, "id", id.ToString());
                    using (var response = await client.GetAsync(_getByIdUrl))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        var serialized = JsonSerializer.Deserialize<Recommendation>(data);
                        if (serialized == null) recommendation = serialized;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return recommendation;
        }

        private async Task<List<Recommendation>> GetByRequestIdTask(int requestId)
        {
            List<Recommendation> recommendations = new();

            try
            {
                using (HttpClient client = new())
                {
                    client.BaseAddress = new Uri(this.BaseUrl);
                    string url = QueryHelpers.AddQueryString(_getByIdUrl, "id", requestId.ToString());
                    using (var response = await client.GetAsync(_getByRequestId))
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        if (!response.IsSuccessStatusCode) throw new Exception(data);
                        var serialized = JsonSerializer.Deserialize<List<Recommendation>>(data);
                        if (serialized == null) recommendations = serialized;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ErrorModule.CustomerSupport, ErrorType.Error, ex.Message);
            }

            return recommendations;
        }

        #endregion

    }
}
