using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.ViewModel
{
    public class ResultDTO<T>
    {
        public bool _isSuccess { get; private set; }
        public List<string> _message { get; private set; }
        public T _data { get; private set; }

        public int _statusCode { get; private set; }

        private ResultDTO(bool isSuccess, string message, T data, int statusCode)
        {
            _isSuccess = isSuccess;
            _message = new List<string>() { message };
            _data = data;
            _statusCode = statusCode;
        }

        private ResultDTO(bool isSuccess, IEnumerable<string> message, T data, int statusCode)
        {
            _isSuccess = isSuccess;
            _message = message.ToList();
            _data = data;
            _statusCode = statusCode;
        }

        public static ResultDTO<T> Success(T data, string message = "", int statusCode = 200)
        {
            return new ResultDTO<T>(true, message, data, statusCode);
        }

        public static ResultDTO<T> Fail(string errorMessage, int statusCode = 500)
        {
            return new ResultDTO<T>(false, errorMessage, default(T), statusCode);
        }

        public static ResultDTO<T> Fail(IEnumerable<string> errorMessages, int statusCodes = 500)
        {
            return new ResultDTO<T>(false, errorMessages, default(T), statusCodes);
        }
    }

    public class ResultDTO
    {
        public bool IsSuccess { get; private set; }
        public List<string> Messages { get; private set; }
        public object Data { get; private set; }

        public int StatusCode { get; private set; }

        private ResultDTO(bool isSuccess, IEnumerable<string> message, object data, int statusCode)
        {
            IsSuccess = isSuccess;
            Messages = message is null ? new List<string>() : message.ToList();
            Data = data;
            StatusCode = statusCode;
        }

        public static ResultDTO Success(object data = null, IEnumerable<string> message = null, int statusCode = 200)
        {
            return new ResultDTO(true, message, data, statusCode);
        }

        public static ResultDTO Fail(IEnumerable<string> errorMessages = null, int statusCodes = 500)
        {
            return new ResultDTO(false, errorMessages, null, statusCodes);
        }
    }
}
