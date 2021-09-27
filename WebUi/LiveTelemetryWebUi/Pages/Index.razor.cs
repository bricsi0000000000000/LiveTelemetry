using LiveTelemetryWebUi.Interfaces;
using LiveTelemetryWebUi.Models;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace LiveTelemetryWebUi.Pages
{
    public partial class Index
    {
        [Inject]
        public IApiService ApiService { get; set; }

        public IEnumerable<LiveSession> Sessions { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Sessions = (await ApiService.GetSessions()).ToList();
        }
    }
}
