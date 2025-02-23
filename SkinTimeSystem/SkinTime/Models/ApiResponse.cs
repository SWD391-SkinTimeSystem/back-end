namespace SkinTime.Models
{
    /// <summary>
    ///     Represent an api JSON response structure.
    /// </summary>
    public class ApiResponse
    {
        public bool Success { get; set; }
        public string? ErrorCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public object? Data { get; set; }

        public ApiResponse() { }
        public ApiResponse(bool success, string message = "", object? data = null, string? error = null)
        {
            this.Success = success;
            this.ErrorCode = error;
            this.Message = message;
            this.Data = data;
        }
    }

    /// <summary>
    ///     Derived from the base class <see cref="ApiResponse"/> to support generic typing.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResponse<T> : ApiResponse
    {
        new public T? Data { get; set; }

        public ApiResponse() : base() { }

        public ApiResponse(bool success, string message = "", T? data = default(T))
            : base(success, message, data)
        { 
        }
    }
}
