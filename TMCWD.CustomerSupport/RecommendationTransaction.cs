using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Runtime.CompilerServices;
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

        private HttpClient _client = new();

        #endregion

        #region constructors

        public RecommendationTransaction() { }

        #endregion

        #region public methods

        public void SetClient(HttpClient client) 
        {
            this._client = client;
        }

        public Recommendation ConvertJsonToRecommendation(string json)
        {
            var serializeOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<Recommendation>(json, serializeOptions) ?? new Recommendation(); 
        }

        public List<Recommendation> ConvertJsonToRecommendations(string json)
        {
            var serializeOptions = new JsonSerializerOptions() { PropertyNameCaseInsensitive = true };
            return JsonSerializer.Deserialize<List<Recommendation>>(json, serializeOptions) ?? new List<Recommendation>();
        }

        public async Task<Recommendation> SaveUpdate(int requestId, int userId, Recommendation recommendation)
        {

            StringBuilder sb = new();

            if (recommendation == null) throw new Exception("Recommendation is empty");

            if (String.IsNullOrEmpty(recommendation.Details.Trim())) sb.AppendLine("Recommendation detail is not provided");
            if (recommendation.RequestId <= 0) sb.AppendLine("No request is provided for this recommendation");

            var response = await _client.GetAsync($"api/{requestId}/SaveUpdate/{userId}");
            var data = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode) throw new Exception(data);

            return ConvertJsonToRecommendation(data);
        }

        public async Task<Recommendation> GetById(int requestId, int id)
        {

            if (id <= 0) throw new Exception("Recommendation id is required");

            var response = await _client.GetAsync($"api/{requestId}/Get/{id}");

            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) throw new Exception(data);

            return ConvertJsonToRecommendation(data);

        }

        public async Task<List<Recommendation>> GetByRequestId(int requestId)
        {
            if (requestId <= 0) throw new Exception("Request id is required to get recommendations");

            var response = await _client.GetAsync($"api/{requestId}/Recommendation/GetByRequestId");

            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            return ConvertJsonToRecommendations(data);

        }

        #endregion

    }
}
