using DataModel.Extensions;

namespace DataModel.Constants
{
    public static class FilePathManager
    {
        public static string ConfigurationFilePath
        {
            get
            {
                return TextManager.CONFIGURATION_FILE.MakePath(TextManager.CONFIGURATION_FILES_FOLDER);
            }
        }

        public static string LogFilePath
        {
            get
            {
                return TextManager.ERROR_MESSAGES_LOG_FILE.MakePath(TextManager.LOGS_FILE_FOLDER);
            }
        }

        public static string GroupFilePath
        {
            get
            {
                return TextManager.GROUPS_FILE.MakePath(TextManager.DEFAULT_FILES_FOLDER);
            }
        }

        public static string PageTemplateFilePath
        {
            get
            {
                return TextManager.PAGE_TEMPLATES_FILE.MakePath(TextManager.DEFAULT_FILES_FOLDER);
            }
        }
    }
}
