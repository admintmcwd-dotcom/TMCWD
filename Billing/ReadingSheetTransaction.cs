using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Text.Json;
using TMCWD.Model.Billing;
using TMCWD.Services;

namespace TMCWD.Billing
{
    public class ReadingSheetTransaction
    {

        #region fields

        private readonly WebService _webService;

        #endregion

        #region constructor

        public ReadingSheetTransaction(WebService webService)
        {
            _webService = webService;
        }

        #endregion

        #region private

        private ReadingSheet ConvertJsonToReadingSheet(string json)
        {
            var serialiazerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            return JsonSerializer.Deserialize<ReadingSheet>(json, serialiazerOptions) ?? new();
        }

        private List<ReadingSheet> ConvertJsonToReadingSheets(string json)
        {
            var serialiazerOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<ReadingSheet>>(json, serialiazerOptions) ?? new();
        }

        #endregion

        #region methods

        public async Task<ReadingSheet> Get(int id)
        {
            var response = await _webService.Client.GetAsync($"api/ReadingSheet/Get/{id}");
            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            return ConvertJsonToReadingSheet(data);
        }

        public async Task<List<ReadingSheet>> GetAll()
        {
            var response = await _webService.Client.GetAsync($"api/ReadingSheet/GetAll");
            var data = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode) return null;

            return ConvertJsonToReadingSheets(data);
        }

        #endregion

    }
}
