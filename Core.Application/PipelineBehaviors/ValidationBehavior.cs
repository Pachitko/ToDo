using Core.Application.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using System.Linq;
using MediatR;

namespace Infrastructure.PipelineBehaviors
{
    // todo: fix and test
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, Response<TResponse>>
        //where TRequest : IRequestWrapper<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<Response<TResponse>> Handle(
            TRequest request, 
            CancellationToken cancellationToken, 
            RequestHandlerDelegate<Response<TResponse>> next)
        {
            ValidationContext<TRequest> context = new(request);
            var errors = _validators
                .Select( x =>  x.Validate(context))
                .SelectMany(x => x.Errors.Select(e => new ResponseError(e.PropertyName, e.ErrorMessage)))
                .Where(x => x is not null);

            if (errors.Any())
            {
                return ResponseResult.Fail<TResponse>(errors);
                //return await Task.FromResult(ResponseResult.Fail<TResponse>(errors));
                //throw new ValidationException("Validation error");
            }

            return await next();
        }
    }
}
