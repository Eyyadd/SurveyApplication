using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Survey.Infrastructure.Abstractions
{
    public class Result
    {
        public Result(bool IsSuccess, Error error)
        {
            if ((IsSuccess && error is not null) || (!IsSuccess && error is null))
                throw new InvalidOperationException("Result is not successful");

            this.Error = error!;
            this.IsSuccess = IsSuccess;

        }

        public bool IsSuccess { get; }
        public Error Error { get; }

        public static Result Success() => new(true, null!);
        public static Result Failure(Error error) => new(false, error);

        public static Result<T> Success<T>(T value) => new(value, true, null!);
        public static Result<T> Failure<T>(Error error) => new(default!, false, error);
    }

    public class Result<T> : Result
    {
        private readonly T _value;
        public Result(T value, bool isSuccess, Error error) : base(isSuccess, error)
        {
            _value = value;
        }

        public T Value => IsSuccess ?
            _value :
            throw new InvalidOperationException("Result is not successful");
    }
}
