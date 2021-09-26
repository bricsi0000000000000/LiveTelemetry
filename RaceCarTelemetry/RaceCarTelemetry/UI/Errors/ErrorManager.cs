using DataModel.Constants;
using MaterialDesignThemes.Wpf;
using System;
using System.IO;
using UI.Managers;

namespace UI.Errors
{
    public static class ErrorManager
    {
        /// <param name="className">The name of the class where the message comes</param>
        /// <param name="errorMessage">This will only be written in log</param>
        public static void ShowErrorMessage(string message, Snackbar snackbar, string className = "", string errorMessage = "")
        {
            ShowMessage(message, snackbar, className, isError: true, errorMessage);
        }

        public static void ShowMessage(string message, Snackbar snackbar, string className = "", bool isError = true, string errorMessage = "")
        {
            if (isError)
            {
                WriteLog(message, className, errorMessage);
            }

            snackbar.Background = isError ? ColorManager.Error : ColorManager.Message;
            snackbar.MessageQueue.Enqueue(message);
        }

        private static void WriteLog(string message, string className, string errorMessage = "")
        {
            using StreamWriter writer = new StreamWriter(FilePathManager.LogFilePath, append: true);
            writer.WriteLine($"[{DateTime.Now}]: {className}\t{message}\t{errorMessage}");
        }
    }
}
