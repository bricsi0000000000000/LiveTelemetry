using DataAccess;
using DataModel.Live;

namespace BusinessLogic
{
    public class ConfigurationBusinessLogic
    {
        public LiveConfiguration LoadLiveConfiguration(string fileName, out string errorMessage)
        {
            return new ConfigurationDataAccess().LoadLiveConfiguration(fileName, out errorMessage);
        }
    }
}
