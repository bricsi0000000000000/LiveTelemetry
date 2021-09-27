using LiveTelemetryWebUi.Interfaces;
using LiveTelemetryWebUi.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace LiveTelemetryWebUi.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient httpClient;

        public ApiService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<IEnumerable<LiveSession>> GetSessions()
        {
            HttpResponseMessage response = await httpClient.GetAsync("api/Session/all").ConfigureAwait(false);
            ConfiguredTaskAwaitable<string> result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
            string resultString = result.GetAwaiter().GetResult();

            return JsonConvert.DeserializeObject<List<LiveSession>>(resultString);
        }
    }
}
