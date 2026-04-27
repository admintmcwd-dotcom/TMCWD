using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TMCWD.Model.CustomerSupport;

namespace TMCWD.CustomerSupport
{
    public class IncidentTransaction
    {

        #region constructors

        public IncidentTransaction()
        {
        }

        #endregion

        #region public methods

        public List<Incident> GetIncidentTypes()
        {
            var res = Task.Run(() => GetIncidents()).GetAwaiter().GetResult();
            return res;
        }

        #endregion

        #region private methods

        private async Task<List<Incident>> GetIncidents()
        {
            using (HttpClient client = new())
            {
                client.BaseAddress = new Uri("https://localhost:5178/");
                using (var response = await client.GetAsync("api/IncidentType/GetTypes"))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();

                        return JsonSerializer.Deserialize<List<Incident>>(data);
                    }
                    else
                    {
                        throw new Exception($"Failed to retrieve incident types. Status code: {response.StatusCode}");
                    }
                }
            }
        }

        #endregion

    }
}
