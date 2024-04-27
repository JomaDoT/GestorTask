namespace GestorTask.Utilitys.Responses;

    public class ErrorHelper<T>
    {
        public static Response<T> Response(T data, int errCode, string msg) 
        {
            return new Response<T>()
            {
                Data = data,
                Error = new Error
                {
                    Code = errCode,
                    Message = msg
                }
            };
        }
    }
    public class Response<T>
    {
        public T Data { get; set; }
        public Error Error { get; set; }

    }
    public class Error
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }

