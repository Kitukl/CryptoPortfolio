namespace CryptoAnalyzer.Portfolio.BLL.Exceptions;

public abstract class Result
{
    public bool isSuccess { get; set; }
    public string Errors { get; set; }
}

public class Result<T> : Result
{
    public T Value { get; set; }
    public static Result<T> Success(T value) => new Result<T>(){ isSuccess = true, Value = value };
    public static Result<T> Failure(string errors) => new Result<T>(){ isSuccess = false, Errors = errors };
}