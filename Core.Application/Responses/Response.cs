using System.Collections.Generic;
using System.Linq;

namespace Core.Application.Responses
{
    public static class ResponseResult
    {
        public static Response<T> Ok<T>(T value = default)
            where T : class
            => new(value, null);

        public static Response<T> Fail<T>(IEnumerable<ResponseError> errors, T value = default)
            where T : class
            => new(value, errors);
    }

    public class Response<T>
        where T : class
    {
        public T Value { get; set; }

        public Response(T value, IEnumerable<ResponseError> errors = null)
        {
            Value = value;
            AddErrors(errors);
        }

        public bool Succeeded => !_errors.Any();

        public IReadOnlyList<ResponseError> Errors => _errors;

        private readonly List<ResponseError> _errors = new();

        public void AddErrors(IEnumerable<ResponseError> errors)
        {
            if (errors != null)
                _errors.AddRange(errors);
        }

        public override string ToString()
        {
            return Succeeded ? "Succeeded" : "Failed";
        }
    }
}