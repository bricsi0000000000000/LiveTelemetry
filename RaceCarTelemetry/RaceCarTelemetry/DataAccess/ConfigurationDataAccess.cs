using DataModel.Constants;
using DataModel.Live;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class ConfigurationDataAccess : BaseDataAccess
    {
        public LiveConfiguration LoadLiveConfiguration(out string errorMessage)
        {
            LiveConfiguration configuration = new LiveConfiguration();

            if (CheckFile(FilePathManager.ConfigurationFilePath, out errorMessage) && CheckFile(FilePathManager.ApiConfigurationFilePath, out errorMessage))
            {
                using StreamReader configurationReader = new StreamReader(FilePathManager.ConfigurationFilePath);
                try
                {
                    configuration = JsonConvert.DeserializeObject<LiveConfiguration>(configurationReader.ReadToEnd());
                }
                catch (JsonReaderException)
                {
                    errorMessage = $"There was a problem reading '{FilePathManager.ConfigurationFilePath}'";
                }


                using StreamReader apiConfigurationReader = new StreamReader(FilePathManager.ApiConfigurationFilePath);
                try
                {
                    dynamic configurationJson = JsonConvert.DeserializeObject(apiConfigurationReader.ReadToEnd());

                    configuration.Sections = new List<LiveSection>()
                    {
                        new LiveSection { Name = ApiCallManager.HEALTH_CHECK, Value = configurationJson.health_check },
                        new LiveSection { Name = ApiCallManager.LIVE_SESSION, Value = configurationJson.get_live_session },
                        new LiveSection { Name = ApiCallManager.ALL_LIVE_SESSIONS, Value = configurationJson.get_all_sessions },
                        new LiveSection { Name = ApiCallManager.POST_NEW_SESSION, Value = configurationJson.post_new_session },
                        new LiveSection { Name = ApiCallManager.CHANGE_SESSION_TO_LIVE, Value = configurationJson.change_session_to_live },
                        new LiveSection { Name = ApiCallManager.CHANGE_SESSION_TO_OFFLINE, Value = configurationJson.change_session_to_offline },
                        new LiveSection { Name = ApiCallManager.CHANGE_SESSION_NAME, Value = configurationJson.change_session_name },
                        new LiveSection { Name = ApiCallManager.DELETE_SESSION, Value = configurationJson.delete_session },
                        new LiveSection { Name = ApiCallManager.ACTIVE_SESSION_SENSORS, Value = configurationJson.active_session_sensors },
                        new LiveSection { Name = ApiCallManager.GET_PACKAGE_BY_ID, Value = configurationJson.get_package_by_id },
                        new LiveSection { Name = ApiCallManager.GET_PACKAGES_AFTER, Value = configurationJson.get_packages_after },
                        new LiveSection { Name = ApiCallManager.GET_ALL_PACKAGES, Value = configurationJson.get_all_packages },
                        new LiveSection { Name = ApiCallManager.GET_ALL_SENSORS, Value = configurationJson.get_all_sensors },
                        new LiveSection { Name = ApiCallManager.GET_SENSOR_NAMES, Value = configurationJson.get_sensor_names },
                    };
                }
                catch (JsonReaderException)
                {
                    errorMessage += $"There was a problem reading '{FilePathManager.ApiConfigurationFilePath}'";
                }
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                configuration = null;
            }

            return configuration;
        }

        public void SaveLiveConfiguration(LiveConfiguration Configuration, out string errorMessage)
        {
            if (CheckFile(FilePathManager.ConfigurationFilePath, out errorMessage))
            {
                using StreamWriter writer = new StreamWriter(FilePathManager.ConfigurationFilePath);

                JsonSerializer serializer = new JsonSerializer();
                try
                {
                    serializer.Serialize(writer, Configuration);
                }
                catch (Exception)
                {
                    errorMessage = "Can't save file!";
                }
            }
        }
    }
}
