using DataModel.Constants;
using System;
using System.IO;

namespace DataAccess
{
    public class ErrorManagerDataAccess : BaseDataAccess
    {
        public void WriteLog(string message, string className, string errorMessage = "")
        {
            if (CheckFile(FilePathManager.LogFilePath, out errorMessage))
            {
                using StreamWriter writer = new StreamWriter(FilePathManager.LogFilePath, append: true);
                writer.WriteLine($"[{DateTime.Now}]: {className}\t{message}\t{errorMessage}");
            }
        }
    }
}
