using DataModel.Constants;
using MaterialDesignThemes.Wpf;
using System;
using System.IO;
using UI.Extensions;
using UI.Managers;

namespace UI.Errors
{
    public enum MessageType
    {
        Error,
        Info
    }

    public static class ErrorManager
    {
        public static void ShowMessage(string message, Snackbar snackbar, MessageType type, string className = "", string exceptionMessage = "")
        {
            switch (type)
            {
                case MessageType.Error:
                    WriteLog(message, className, exceptionMessage);
                    snackbar.Background = ColorManager.Error;
                    break;
                case MessageType.Info:
                    snackbar.Background = ColorManager.Secondary.ConvertBrush();
                    break;
            }

            snackbar.MessageQueue.Enqueue(message);
        }

        private static void WriteLog(string message, string className, string errorMessage = "")
        {
            using StreamWriter writer = new StreamWriter(FilePathManager.LogFilePath, append: true);
            writer.WriteLine($"[{DateTime.Now}]: {className}\t{message}\t{errorMessage}");
        }
    }
}
