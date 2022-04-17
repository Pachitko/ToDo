using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Core.Application.Responses;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Core.Application.Helpers;
using MediatR;

namespace ToDoApi.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();

        public override ActionResult ValidationProblem()
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        protected ActionResult ResponseFailed<T>(Response<T> response)
        {
            if (response.Errors.Count > 0)
            {
                ModelState.AddModelErrors(response.Errors);
                return ValidationProblem();
            }
            else if (response.Value is null)
            {
                return NotFound();
            }
            else
            {
                return BadRequest();
            }
        }

        protected string CreateResourcePageUri(IHasPageParametersAndOrderByAndFields query, string routeName, ResourcePageUriType type)
        {
            return type switch
            {
                ResourcePageUriType.PreviousPage => Url.Link(routeName, new
                {
                    pageNumber = query.PageNumber - 1,
                    pageSize = query.PageSize,
                    orderBy = query.OrderBy,
                    fields = query.Fields
                }),
                ResourcePageUriType.NextPage => Url.Link(routeName, new
                {
                    pageNumber = query.PageNumber + 1,
                    pageSize = query.PageSize,
                    orderBy = query.OrderBy,
                    fields = query.Fields
                }),
                ResourcePageUriType.Current => Url.Link(routeName, new
                {
                    pageNumber = query.PageNumber,
                    pageSize = query.PageSize,
                    orderBy = query.OrderBy,
                    fields = query.Fields
                }),
                _ => throw new System.NotImplementedException(),
            };
        }
    }
}
