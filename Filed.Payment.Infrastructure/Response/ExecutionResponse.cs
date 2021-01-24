using Filed.Payments.Interfaces;

namespace Filed.Payments.Infrastructure.Response
{
    public class ExecutionResponse<T> : IExecutionResponse<T> where T : class
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public int StatusCode { get; set; }

        public T Data { get; set; }
    }
}
