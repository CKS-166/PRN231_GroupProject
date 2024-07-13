using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FPTU_Starter.API.Exception
{
    public class GlobalExceptionHandler : ExceptionFilterAttribute
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public override void OnException(ExceptionContext context)
        {
            // Lấy log ExceptionError
            var exceptionError = context.Exception as ExceptionError;

            // Nếu là ExceptionError, trả về StatusCode và Message
            if (exceptionError != null)
            {
                context.Result = new JsonResult(new
                {
                    StatusCode = exceptionError.StatusCode,
                    Message = exceptionError.Message
                });
                context.HttpContext.Response.StatusCode = exceptionError.StatusCode;
            }
            else
            {
                // Nếu không phải ExceptionError, trả về StatusCode 500 và Message
                context.Result = new JsonResult(new
                {
                    StatusCode = 500,
                    Message = context.Exception.Message
                });
                context.HttpContext.Response.StatusCode = 500;
            }
        }
    }
}
