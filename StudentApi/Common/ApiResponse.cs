namespace StudentApi.Common;

public class ApiResponse<T>
{
    public string Message { get; set; } = default!;
    public string ResponseCode { get; set; } = default!;
    public T? Data { get; set; }
    public DateTime ResponseTime { get; set; } = DateTime.UtcNow;
}
