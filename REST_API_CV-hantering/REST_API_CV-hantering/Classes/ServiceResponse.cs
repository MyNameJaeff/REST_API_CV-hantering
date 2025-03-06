namespace REST_API_CV_hantering.Classes
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }
        public int StatusCode { get; set; }

        public static ServiceResponse<T> CreateSuccess(T data, string? message = null)
            => new() { Success = true, Data = data, StatusCode = 200, Message = message };

        public static ServiceResponse<T> CreateFail(string message, int statusCode = 400)
            => new() { Success = false, Message = message, StatusCode = statusCode };
    }
}
