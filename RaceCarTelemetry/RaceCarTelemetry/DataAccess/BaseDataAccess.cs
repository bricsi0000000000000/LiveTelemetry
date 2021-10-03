using System.IO;
using System.Linq;

namespace DataAccess
{
    public abstract class BaseDataAccess
    {
        protected virtual bool CheckFile(string fileName, out string errorMessage)
        {
            if(!IsFileExists(fileName, out errorMessage))
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
    }
}
