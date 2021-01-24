using Filed.Payments.Interfaces;

namespace Filed.Payments.Infrastructure.Response
{
    public class ResponseFactory
    {
        public IExecutionResponse<T> ExecutionResponse<T>(string message, T data = null, bool status = false, int statusCode = 200) where T : class
        {
            return new ExecutionResponse<T>
            {
                Status = status,
                Message = message,
                Data = data,
                StatusCode = statusCode
            };
        }
    }
}
