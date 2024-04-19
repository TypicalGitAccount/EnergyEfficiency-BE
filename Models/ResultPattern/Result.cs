namespace EnergyEfficiencyBE.Models.ResultPattern
{
    public class Result
    {
        public bool IsSuccess { get; }
        public string? Message { get; }
        public bool IsFailure => !IsSuccess;

        protected Result(bool isSuccess, string? message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }

        public static Result Fail(string message)
        {
            return new Result(false, message);
        }

        public static Result<T> Fail<T>(string message)
        {
            return new Result<T>(default, false, message);
        }

        public static Result Ok(string? message = null)
        {
            return new Result(true, message);
        }

        public static Result<T> Ok<T>(T value, string? message = null)
        {
            return new Result<T>(value, true, message);
        }
    }
}
