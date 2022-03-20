using Core.Application.Responses;
using MediatR;

namespace Core.Application.Features
{
    public interface IHandlerWrapper<in TIn, TOut> : IRequestHandler<TIn, Response<TOut>>
        where TIn : IRequestWrapper<TOut>
        //where TOut : class
    {
    }
}