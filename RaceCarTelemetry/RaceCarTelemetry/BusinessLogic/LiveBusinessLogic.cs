using DataAccess;
using DataModel.Live;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class LiveBusinessLogic
    {
        public async Task<bool> HealthCheck(HttpClient client, string apiCall)
        {
            try
            {
                return await new LiveDataAccess().HealthCheck(client, apiCall);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<LiveSession>> GetAllLiveSessions(HttpClient client, string apiCall)
        {
            try
            {
                return await new LiveDataAccess().GetAllLiveSessions(client, apiCall);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<string>> GetActiveSessionSensors(HttpClient client, string apiCall)
        {
            try
            {
                return await new LiveDataAccess().GetActiveSessionSensors(client, apiCall);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> ChangeSessionName(HttpClient client, LiveSession session, string apiCall)
        {
            try
            {
                return await new LiveDataAccess().ChangeSessionName(client, session, apiCall);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> ChangeSessionState(HttpClient client, bool toLive, int selectedSessionId, string apiCall)
        {
            try
            {
                return await new LiveDataAccess().ChangeSessionState(client, toLive, selectedSessionId, apiCall);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> DeleteSession(HttpClient client, string apiCall)
        {
            try
            {
                return await new LiveDataAccess().DeleteSession(client, apiCall);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<int> AddSession(HttpClient client, LiveSession session, string apiCall)
        {
            try
            {
                return await new LiveDataAccess().AddSession(client, session, apiCall);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<SensorValue>> GetPackagesSensorValues(HttpClient client, string apiCall)
        {
            try
            {
                return await new LiveDataAccess().GetPackagesSensorValues(client, apiCall);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<Sensor>> GetAllSensors(HttpClient client, string apiCall)
        {
            try
            {
                return await new LiveDataAccess().GetAllSensors(client, apiCall);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        public async Task<List<string>> GetSensorNames(HttpClient client, string apiCall)
        {
            try
            {
                return await new LiveDataAccess().GetSensorNames(client, apiCall);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
