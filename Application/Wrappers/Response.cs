using Application.Constants;
using Application.Helpers;

namespace Application.Wrappers
{
    public class Response<T>
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string TraceId { get; set; }
        public List<string> Errors { get; set; }
        public T Data { get; set; }

        public Response()
        {
            Errors = new List<string>();
        }

        public Response(T data, string message = null)
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
        public Response(T data, int status, string message = null)
        {

            Succeeded = true;
            Message = message;
            Data = data;
            TraceId = TraceHelper.GetTraceId();
            Errors = new List<string>();
        }

        public static Response<T> SuccessResponse(T data)
        {
            return new Response<T>(data, ApiMessages.Success());
        }

        public static Response<T> CreatedSuccessResponse(T data)
        {
            return new Response<T>(data, ApiMessages.Success());
        }

        public static Response<T> UpdatedSuccessResponse(T data)
        {
            return new Response<T>(data, ApiMessages.UpdatedSuccess());
        }

        public static Response<T> SuccessResponse(T data, string message)
        {
            return new Response<T>(data, message);
        }

        public static Response<T> DeleteSuccessResponse(T data)
        {
            return new Response<T>(data, ApiMessages.DeleteSuccess());
        }

        public static Response<T> FailResponse(string message)
        {
            return new Response<T> { Message = message, Succeeded = false };
        }

        public static Response<T> FailResponseMessage(T data)
        {
            return new Response<T>(data, ApiMessages.NotFoundMessage());
        }
    }
}
