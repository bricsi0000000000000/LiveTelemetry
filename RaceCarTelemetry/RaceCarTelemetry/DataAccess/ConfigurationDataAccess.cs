using DataModel.Constants;
using DataModel.Live;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class ConfigurationDataAccess : BaseDataAccess
    {
        public LiveConfiguration LoadLiveConfiguration(string fileName, out string errorMessage)
        {
            LiveConfiguration configuration = new LiveConfiguration();

            if (CheckFile(fileName, out errorMessage))
            {
                using StreamReader reader = new StreamReader(fileName);
                try
                {
                    dynamic configurationJson = JsonConvert.DeserializeObject(reader.ReadToEnd());

                    configuration.WaitBetweenCollectData = configurationJson.live.wait_between_collect_data;
                    configuration.IsHttps = configurationJson.live.isHTTPS;
                    configuration.Url = configurationJson.live.url;
                    configuration.Port = configurationJson.live.port;
                    configuration.TimeOut = configurationJson.live.time_out;

                    configuration.Sections = new List<LiveSection>()
                    {
                        new LiveSection { Name = ApiCallManager.HEALTH_CHECK, Value = configurationJson.live.sections.health_check },
                        new LiveSection { Name = ApiCallManager.LIVE_SESSION, Value = configurationJson.live.sections.get_live_session },
                        new LiveSection { Name = ApiCallManager.ALL_LIVE_SESSIONS, Value = configurationJson.live.sections.get_all_sessions },
                        new LiveSection { Name = ApiCallManager.POST_NEW_SESSION, Value = configurationJson.live.sections.post_new_session },
                        new LiveSection { Name = ApiCallManager.CHANGE_SESSION_TO_LIVE, Value = configurationJson.live.sections.change_session_to_live },
                        new LiveSection { Name = ApiCallManager.CHANGE_SESSION_TO_OFFLINE, Value = configurationJson.live.sections.change_session_to_offline },
                        new LiveSection { Name = ApiCallManager.CHANGE_SESSION_NAME, Value = configurationJson.live.sections.change_session_name },
                        new LiveSection { Name = ApiCallManager.DELETE_SESSION, Value = configurationJson.live.sections.delete_session },
                        new LiveSection { Name = ApiCallManager.ACTIVE_SESSION_SENSORS, Value = configurationJson.live.sections.active_session_sensors },
                        new LiveSection { Name = ApiCallManager.GET_PACKAGE_BY_ID, Value = configurationJson.live.sections.get_package_by_id },
                        new LiveSection { Name = ApiCallManager.GET_PACKAGES_AFTER, Value = configurationJson.live.sections.get_packages_after },
                        new LiveSection { Name = ApiCallManager.GET_ALL_PACKAGES, Value = configurationJson.live.sections.get_all_packages },
                        new LiveSection { Name = ApiCallManager.GET_ALL_SENSORS, Value = configurationJson.live.sections.get_all_sensors },
                    };
                }
                catch (JsonReaderException)
                {
                    errorMessage = $"There was a problem reading '{fileName}'";
                }
            }

            return configuration;
        }
    }
}
