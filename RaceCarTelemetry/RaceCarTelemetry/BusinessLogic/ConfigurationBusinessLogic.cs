using DataAccess;
using DataModel.Live;

namespace BusinessLogic
{
    public class ConfigurationBusinessLogic
    {
        public LiveConfiguration LoadLiveConfiguration(out string errorMessage)
        {
            return new ConfigurationDataAccess().LoadLiveConfiguration(out errorMessage);
        }

        public void SaveLiveConfiguration(LiveConfiguration Configuration, out string errorMessage)
        {
            new ConfigurationDataAccess().SaveLiveConfiguration(Configuration, out errorMessage);
        }
    }
}
