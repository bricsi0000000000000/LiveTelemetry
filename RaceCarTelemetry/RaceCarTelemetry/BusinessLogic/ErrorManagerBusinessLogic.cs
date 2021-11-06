using DataAccess;

namespace BusinessLogic
{
    public class ErrorManagerBusinessLogic
    {
        private ErrorManagerDataAccess errorManagerDataAccess;

        public ErrorManagerBusinessLogic()
        {
            errorManagerDataAccess = new ErrorManagerDataAccess();
        }

        public void WriteLog(string message, string className, string errorMessage = "")
        {
            errorManagerDataAccess.WriteLog(message, className, errorMessage);
        }
    }
}
