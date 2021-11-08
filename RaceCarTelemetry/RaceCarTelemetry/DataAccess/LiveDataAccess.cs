using DataModel.Live;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataAccess
{
    public class LiveDataAccess : BaseDataAccess
    {
        public async Task<bool> HealthCheck(HttpClient client, string apiCall)
        {
            return await CallGetApi<bool>(client, apiCall);
        }

        public async Task<List<LiveSession>> GetAllLiveSessions(HttpClient client, string apiCall)
        {
            return await CallGetApi<List<LiveSession>>(client, apiCall);
        }

        public async Task<List<string>> GetActiveSessionSensors(HttpClient client, string apiCall)
        {
            return await CallGetApi<List<string>>(client, apiCall);
        }

        /// <returns>Result code</returns>
        public async Task<int> ChangeSessionName(HttpClient client, LiveSession session, string apiCall)
        {
            return await CallPutApi(client, apiCall, session);
        }

        /// <returns>Result code</returns>
        public async Task<int> ChangeSessionState(HttpClient client, int selectedSessionId, string apiCall)
        {
            return await CallPutApi(client, apiCall, selectedSessionId);
        }

        /// <returns>Result code</returns>
        public async Task<int> DeleteSession(HttpClient client, string apiCall)
        {
            return await CallDeleteApi(client, apiCall);
        }

        /// <returns>Result code</returns>
        public async Task<int> AddSession(HttpClient client, LiveSession session, string apiCall)
        {
            return await CallPostApi(client, apiCall, session);
        }

        public async Task<List<SensorValue>> GetPackagesSensorValues(HttpClient client, string apiCall)
        {
            return await CallGetApi<List<SensorValue>>(client, apiCall);
        }

        public async Task<List<Sensor>> GetAllSensors(HttpClient client, string apiCall)
        {
            return await CallGetApi<List<Sensor>>(client, apiCall);
        }

        public async Task<List<string>> GetSensorNames(HttpClient client, string apiCall)
        {
            return await CallGetApi<List<string>>(client, apiCall);
        }
    }
}
