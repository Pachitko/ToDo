using Core.Application.Features;
using System.Threading.Tasks;
using System.Threading;
using MediatR;
using System;

namespace Core.Application.PipelineBehaviors
{
    public class UserIdResolver<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IWithUserId
        where TResponse : class
    {
        public UserIdResolver()
        {

        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            request.UserId = Guid.NewGuid();

            return await next();
        }
    }
}
