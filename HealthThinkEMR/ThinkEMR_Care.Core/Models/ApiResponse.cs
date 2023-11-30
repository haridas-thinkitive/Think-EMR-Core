namespace ThinkEMR_Care.Core.Models
{
    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }

        public string? Message { get; set; }

        public int StatusCode { get; set; }

        public T? Response { get; set; }

        public string? CurrentUserNmae { get; set; }

    }
}
