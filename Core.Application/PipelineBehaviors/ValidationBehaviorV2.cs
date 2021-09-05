﻿using FluentValidation;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.PipelineBehaviors
{
    public class ValidationBehaviorV2<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IValidateable
        where TResponse : class
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;
        public ValidationBehaviorV2(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_validators.Any())
            {
                var context = new ValidationContext<TRequest>(request);
                var validationResults = await Task.WhenAll(_validators.Select(v => v.ValidateAsync(context, cancellationToken)));
                var failures = validationResults.SelectMany(r => r.Errors).Where(f => f != null).ToList();
                if (failures.Count != 0)
                    throw new FluentValidation.ValidationException(failures);
            }
            return await next();
        }
    }
}