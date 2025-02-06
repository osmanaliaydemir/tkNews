namespace tkNews.Application.Common.Models;

public class BaseResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string> Errors { get; set; }
    
    public BaseResponse()
    {
        IsSuccess = true;
        Errors = new List<string>();
    }
    
    public static BaseResponse<T> Success(T data, string message = "")
    {
        return new BaseResponse<T>
        {
            IsSuccess = true,
            Message = message,
            Data = data
        };
    }
    
    public static BaseResponse<T> Failure(string message, List<string>? errors = null)
    {
        return new BaseResponse<T>
        {
            IsSuccess = false,
            Message = message,
            Errors = errors ?? new List<string>()
        };
    }
} 