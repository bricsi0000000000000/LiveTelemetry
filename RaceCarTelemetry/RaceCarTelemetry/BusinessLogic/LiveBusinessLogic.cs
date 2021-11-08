using DataAccess;
using DataModel.Live;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class LiveBusinessLogic
    {
        public async Task<bool> HealthCheck(HttpClient client, string apiCall)
        {
            return await new LiveDataAccess().HealthCheck(client, apiCall);
        }

        public async Task<List<LiveSession>> GetAllLiveSessions(HttpClient client, string apiCall)
        {
            return await new LiveDataAccess().GetAllLiveSessions(client, apiCall);
        }

        public async Task<List<string>> GetActiveSessionSensors(HttpClient client, string apiCall)
        {
            return await new LiveDataAccess().GetActiveSessionSensors(client, apiCall);
        }

        public async Task<int> ChangeSessionName(HttpClient client, LiveSession session, string apiCall)
        {
            return await new LiveDataAccess().ChangeSessionName(client, session, apiCall);
        }

        public async Task<int> ChangeSessionState(HttpClient client, int selectedSessionId, string apiCall)
        {
            return await new LiveDataAccess().ChangeSessionState(client, selectedSessionId, apiCall);
        }

        public async Task<int> DeleteSession(HttpClient client, string apiCall)
        {
            return await new LiveDataAccess().DeleteSession(client, apiCall);
        }

        public async Task<int> AddSession(HttpClient client, LiveSession session, string apiCall)
        {
            return await new LiveDataAccess().AddSession(client, session, apiCall);
        }

        public async Task<List<SensorValue>> GetPackagesSensorValues(HttpClient client, string apiCall)
        {
            return await new LiveDataAccess().GetPackagesSensorValues(client, apiCall);
        }

        public async Task<List<Sensor>> GetAllSensors(HttpClient client, string apiCall)
        {
            return await new LiveDataAccess().GetAllSensors(client, apiCall);
        }

        public async Task<List<string>> GetSensorNames(HttpClient client, string apiCall)
        {
            return await new LiveDataAccess().GetSensorNames(client, apiCall);
        }
    }
}
