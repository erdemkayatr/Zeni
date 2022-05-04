namespace Zeni.Infra.Results
{
    public class Result : IResult
    {
        public bool Succeeded { get; set; }
        public string Message { get; set; }

        public static IResult Success()
        {
            return new Result
            {
                Succeeded = true,
            };
        }
        public static Task<IResult> SuccessAsync()
        {
            return Task.FromResult(Success());
        }

        public static Task<IResult> SuccessAsync(string message)
        {
            return Task.FromResult(Success(message));

        }

        public static IResult Success(string message)
        {
            return new Result { Succeeded = true, Message = message };
        }
    }

    public class Result<T> : Result, IResult<T>
    {
        public T ResultData { get; set; }

        public static new IResult<T> SuccessAsync()
        {
            return new Result<T>
            {
                Succeeded = true,
            };
        }

        public static new Task<Result<T>> SuccessAsync(string message)
        {
            return Task.FromResult(new Result<T>
            {
                Succeeded = true,
                Message = message
            });
        }

        public static new Result<T> Success(string message)
        {
            return new Result<T>
            {
                Succeeded = true,
                Message = message
            };
        }
    }
}