using Core.Application.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using FluentValidation;
using System.Linq;
using MediatR;

namespace Core.Application.PipelineBehaviors
{
    public class ValidationBehaviorV3<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>, IValidateable
        where TResponse : class
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehaviorV3(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            CancellationToken cancellationToken, 
            RequestHandlerDelegate<TResponse> next)
        {
            ValidationContext<TRequest> context = new(request);
            var errors = _validators
                .Select(x =>  x.Validate(context))
                .SelectMany(x => x.Errors.Select(e => new ResponseError(e.PropertyName, e.ErrorMessage)))
                .Where(x => x is not null);

            if (errors.Any())
            {
                //Activator.CreateInstance<>
                //return ResponseResult.Fail<TResponse>(errors);
                //return await Task.FromResult(ResponseResult.Fail<TResponse>(errors));
                //throw new ValidationException("Validation error");
            }

            return await next();
        }
    }
}
