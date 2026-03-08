namespace MemberApi.Models
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public string? Message { get; set; }

        public ApiResponse(bool success, T? data, string? message = null)
        {
            Success = success;
            Data = data;
            Message = message;
        }
    }
}