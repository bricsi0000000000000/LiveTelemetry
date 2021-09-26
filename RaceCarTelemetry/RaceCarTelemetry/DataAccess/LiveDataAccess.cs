using DataModel.Constants;
using DataModel.Live;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccess
{
    public class LiveDataAccess : BaseDataAccess
    {
        public async Task<bool> HealthCheck(HttpClient client, string apiCall)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiCall).ConfigureAwait(false);
                ConfiguredTaskAwaitable<string> result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string resultString = result.GetAwaiter().GetResult();

                return JsonConvert.DeserializeObject<bool>(resultString);
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
                HttpResponseMessage response = await client.GetAsync(apiCall).ConfigureAwait(false);
                ConfiguredTaskAwaitable<string> result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string resultString = result.GetAwaiter().GetResult();

                return JsonConvert.DeserializeObject<List<LiveSession>>(resultString);
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
                HttpResponseMessage response = await client.GetAsync(apiCall).ConfigureAwait(false);
                ConfiguredTaskAwaitable<string> result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string resultString = result.GetAwaiter().GetResult();

                return JsonConvert.DeserializeObject<List<string>>(resultString);
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
                HttpResponseMessage response = await client.PutAsJsonAsync(apiCall, session).ConfigureAwait(false);
                ConfiguredTaskAwaitable<string> result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                int resultCode = int.Parse(result.GetAwaiter().GetResult());
                return resultCode;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <param name="toLive">If true change the state to live otherwise to false</param>
        /// <returns>Result code</returns>
        public async Task<int> ChangeSessionState(HttpClient client, bool toLive, int selectedSessionId, string apiCall)
        {
            try
            {
                HttpResponseMessage response;

                if (toLive)
                {
                    response = await client.PutAsJsonAsync(apiCall, selectedSessionId).ConfigureAwait(false);
                }
                else
                {
                    response = await client.PutAsJsonAsync(apiCall, selectedSessionId).ConfigureAwait(false);
                }

                ConfiguredTaskAwaitable<string> result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                int resultCode = int.Parse(result.GetAwaiter().GetResult());

                return resultCode;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <returns>Result code</returns>
        public async Task<int> DeleteSession(HttpClient client, string apiCall)
        {
            try
            {
                HttpResponseMessage response = await client.DeleteAsync(apiCall).ConfigureAwait(false);
                ConfiguredTaskAwaitable<string> result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                int resultCode = int.Parse(result.GetAwaiter().GetResult());

                return resultCode;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        /// <returns>Result code</returns>
        public async Task<int> AddSession(HttpClient client, LiveSession session, string apiCall)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(apiCall, session).ConfigureAwait(false);
                ConfiguredTaskAwaitable<string> result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                int resultCode = int.Parse(result.GetAwaiter().GetResult());

                return resultCode;
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
                HttpResponseMessage response = await client.GetAsync(apiCall).ConfigureAwait(false);
                ConfiguredTaskAwaitable<string> result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string resultString = result.GetAwaiter().GetResult();
                dynamic packagesSensorValues = JsonConvert.DeserializeObject(resultString);

                List<SensorValue> sensorValues = new List<SensorValue>();

                if (packagesSensorValues != null)
                {
                    for (int index = 0; index < packagesSensorValues.Count; index++)
                    {
                        SensorValue value = new SensorValue
                        {
                            Value = packagesSensorValues[index].value,
                            SensorId = packagesSensorValues[index].sensorId,
                            SessionId = packagesSensorValues[index].sessionId,
                            PackageId = packagesSensorValues[index].packageId
                        };

                        sensorValues.Add(value);
                    }
                }

                return sensorValues;
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
                HttpResponseMessage response = await client.GetAsync(apiCall).ConfigureAwait(false);
                ConfiguredTaskAwaitable<string> result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string resultString = result.GetAwaiter().GetResult();

                return JsonConvert.DeserializeObject<List<Sensor>>(resultString);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }
    }
}
