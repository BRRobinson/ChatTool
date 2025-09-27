namespace ChatTool.API.Models;

public class ReturnResult
{
    public bool IsSuccess { get; set; } = true;
    public string Message { get; set; } = string.Empty;

    public ReturnResult(string message = "", bool isSuccess = true)
    {
        Message = message;
        IsSuccess = isSuccess;
    }

    public static ReturnResult Success(string message = "Success") =>
        new ReturnResult(message);

    public static ReturnResult Failed(string message = "Failed") =>
        new ReturnResult(message, false);

}

public class ReturnResult<T> : ReturnResult
{
    public T? Value { get; set; }

    public ReturnResult(T value, string message = "", bool isSuccess = true) : base(message, isSuccess)
    {
        Value = value;
    }

    public static ReturnResult<T> Success(T value, string message = "Success") =>
        new ReturnResult<T>(value, message, true);

    public static ReturnResult<T> Failed(T value = default!, string message = "Failed") =>
        new ReturnResult<T>(value, message, false);

}
