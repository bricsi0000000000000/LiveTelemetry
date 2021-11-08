using BusinessLogic;
using DataModel.Live;

namespace UI.Managers
{
    public static class ConfigurationManager
    {
        public static LiveConfiguration Configuration { get; set; }

        public static void Save(ConfigurationBusinessLogic configurationBusinessLogic, out string errorMessage)
{
            configurationBusinessLogic.SaveLiveConfiguration(Configuration, out errorMessage);
        }
    }
}
