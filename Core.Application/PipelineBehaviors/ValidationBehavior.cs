using Core.Application.Responses;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.PipelineBehaviors
{
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IValidateable
        where TResponse : class // todo may not work with value type results
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        private readonly ILogger<TRequest> _logger;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators, ILogger<TRequest> logger)
        {
            _validators = validators;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //var validationResult = await _compositeValidator.ValidateAsync(request, cancellationToken);

            //ValidationContext<TRequest> context = new(request);
            //var errors = _validators
            //    .Select(x => x.Validate(context))
            //    .SelectMany(x => x.Errors.Select(e => new ResponseError(e.PropertyName, e.ErrorMessage)))
            //    .Where(x => x is not null);

            ValidationContext<TRequest> context = new(request);
            var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
            var errors = validationResults.
                SelectMany(x => x.Errors.Select(e => new ResponseError(e.PropertyName, e.ErrorMessage)))
                .Where(f => f is not null);

            if (errors.Any())
            {
                //_logger.LogError(EventIDs.EventIdPipelineThrown,
                //    MessageTemplates.ValidationErrorsLog,
                //    result.Errors.Select(s => s.ErrorMessage).Aggregate((acc, curr) => acc += string.Concat("_|_", curr)),
                //    _currentUser.UserName
                //    );

                var responseType = typeof(TResponse);

                if (responseType.IsGenericType)
                {
                    var resultType = responseType.GetGenericArguments()[0];
                    var invalidResponseType = typeof(Response<>).MakeGenericType(resultType);

                    var invalidResponse = Activator.CreateInstance(invalidResponseType, null, false, errors) as TResponse;

                    //return ResponseResult.Fail<>(result.Errors.Select(x => new ResponseError(x.ErrorCode, x.ErrorMessage)), default) as TResponse;
                    return invalidResponse;
                }
            }

            var response = await next();

            return response;
        }
    }
}