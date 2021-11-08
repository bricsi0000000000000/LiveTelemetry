using BusinessLogic;
using MaterialDesignThemes.Wpf;
using System;
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
        private static ErrorManagerBusinessLogic errorManagerBusinessLogic;
        private static bool isInitialized = false;
        public static void Initialize()
        {
            errorManagerBusinessLogic = new ErrorManagerBusinessLogic();
            isInitialized = true;
        }

        public static void ShowMessage(string message, Snackbar snackbar, MessageType type, string className = "", string exceptionMessage = "")
        {
            CheckIfInitialized();

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

        public static void WriteLog(string message, string className, string errorMessage = "")
        {
            CheckIfInitialized();

            errorManagerBusinessLogic.WriteLog(message, className, errorMessage);
        }

        private static void CheckIfInitialized()
        {
            if (!isInitialized)
            {
                throw new Exception("Error manager is not initialized");
            }
        }
    }
}
