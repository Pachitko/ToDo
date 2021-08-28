using Core.Application.Responses;
using MediatR;
    
namespace Core.Application.Features
{
    public interface IRequestWrapper<T> : IRequest<Response<T>>
    {
    }
}