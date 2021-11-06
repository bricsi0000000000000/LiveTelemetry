using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DataAccess
{
    public abstract class BaseDataAccess
    {
        protected virtual bool CheckFile(string fileName, out string errorMessage)
        {
            if (!IsFileExists(fileName, out errorMessage))
            {
                return false;
            }

            if (!IsFileEmpty(fileName, out errorMessage))
            {
                return false;
            }

            errorMessage = string.Empty;

            return true;
        }

        protected virtual bool IsFileExists(string fileName, out string errorMessage)
        {
            if (!File.Exists(fileName))
            {
                errorMessage = $"File '{fileName.Split('/').Last()}' not found!";
                return false;
            }
            else
            {
                errorMessage = string.Empty;
                return true;
            }
        }

        protected virtual bool IsFileEmpty(string fileName, out string errorMessage)
        {
            if (new FileInfo(fileName).Length == 0)
            {
                errorMessage = $"'{fileName}' is empty";
                return false;
            }
            else
            {
                errorMessage = string.Empty;
                return true;
            }
        }

        protected virtual async Task<T> CallGetApi<T>(HttpClient client, string apiCall)
        {
            try
            {
                HttpResponseMessage response = await client.GetAsync(apiCall).ConfigureAwait(false);
                ConfiguredTaskAwaitable<string> result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                string resultString = result.GetAwaiter().GetResult();

                return JsonConvert.DeserializeObject<T>(resultString);
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected virtual async Task<int> CallPutApi(HttpClient client, string apiCall, object value)
        {
            try
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(apiCall, value).ConfigureAwait(false);
                ConfiguredTaskAwaitable<string> result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                int resultCode = int.Parse(result.GetAwaiter().GetResult());

                return resultCode;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected virtual async Task<int> CallPostApi(HttpClient client, string apiCall, object value)
        {
            try
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(apiCall, value).ConfigureAwait(false);
                ConfiguredTaskAwaitable<string> result = response.Content.ReadAsStringAsync().ConfigureAwait(false);
                int resultCode = int.Parse(result.GetAwaiter().GetResult());

                return resultCode;
            }
            catch (Exception exception)
            {
                throw exception;
            }
        }

        protected virtual async Task<int> CallDeleteApi(HttpClient client, string apiCall)
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
    }
}
