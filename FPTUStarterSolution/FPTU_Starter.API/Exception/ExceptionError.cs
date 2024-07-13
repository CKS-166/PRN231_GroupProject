namespace FPTU_Starter.API.Exception
{
    public class ExceptionError : System.Exception
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public ExceptionError(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
