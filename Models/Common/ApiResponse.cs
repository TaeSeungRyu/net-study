namespace MemberApi.Models.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }

        public ApiResponse() { }

        public ApiResponse(bool success, T? data, string? message = null)
        {
            Success = success;
            Data = data;
            Message = message;
        }

        public static ApiResponse<T> Ok(T? data, string? message = null)
            => new(true, data, message);

        public static ApiResponse<T> Fail(string message)
            => new(false, default, message);
    }
}
