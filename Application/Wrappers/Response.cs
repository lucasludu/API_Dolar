using Application.Helpers;

namespace Application.Wrappers
{
    public class Response<T>
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public string? TraceId { get; set; }
        public List<string>? Errors { get; set; }
        public T? Data { get; set; }

        public Response()
        {
            Errors = new List<string>();
        }

        public Response(T data, string? message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
            TraceId = TraceHelper.GetTraceId();
            Errors = new List<string>();
        }

        public Response(string message)
        {
            Succeeded = false;
            Message = message;
            TraceId = TraceHelper.GetTraceId();
            Errors = new List<string>();
        }

        public Response(string message, List<string> errors)
        {
            Succeeded = false;
            Message = message;
            TraceId = TraceHelper.GetTraceId();
            Errors = errors ?? new List<string>();
        }
    }
}
