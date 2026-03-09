namespace MemberApi.Models
{
    public class ApiResponse<T>
    {
        public bool success { get; set; }
        public T? data { get; set; }
        public string? message { get; set; }

        public ApiResponse(bool success, T? data, string? message = null)
        {
            this.success = success;
            this.data = data;
            this.message = message;
        }
    }
}