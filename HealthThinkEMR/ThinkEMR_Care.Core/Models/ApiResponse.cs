namespace ThinkEMR_Care.Core.Models
{
    public class APIResponce<T>
    {
        public bool IsSuccess { get; set; }

        public string? Message { get; set; }

        public int StatusCode { get; set; }

        public T? Responce { get; set; }
    }
}
