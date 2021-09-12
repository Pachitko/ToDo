using System.Collections.Generic;

namespace Core.Application.Responses
{
    public class Response<T> 
        //where T : class
    {
        public static Response<T> Ok(T value = default)
           => new(value, true, null);

        public static Response<T> Fail(IEnumerable<ResponseError> errors = null, T value = default)
            => new(value, false, errors);

        public T Value { get; set; }

        public bool Succeeded { get; init; } //!_errors.Any();

        private readonly List<ResponseError> _errors = new();

        public IReadOnlyList<ResponseError> Errors => _errors;

        public Response(T value, bool succeeded, IEnumerable<ResponseError> errors)
        {
            Succeeded = succeeded;
            Value = value;
            AddErrors(errors);
        }

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