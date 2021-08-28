using System.Collections.Generic;

namespace Core.Application.Responses
{
    public static class ResponseResult
    {
        public static Response<T> Ok<T>(T value = default)
            => new(value, true, null);

        public static Response<T> Fail<T>(IEnumerable<ResponseError> errors, T value = default)
            => new(value, false, errors);
    }

    public class Response<T>
    {
        public Response(T value, bool succeeded, IEnumerable<ResponseError> errors)
        {
            Value = value;
            Succeeded = succeeded;
            AddErrors(errors);
        }

        public T Value { get; set; }

        public bool Succeeded { get; protected set; }

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