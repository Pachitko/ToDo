using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Core.Application.Features;

namespace ToDoApi.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        public override ActionResult ValidationProblem()
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        protected string CreateResourcePageUri(PageParameters query, string routeName, ResourcePageUriType type)
        {
            return type switch
            {
                ResourcePageUriType.PreviousPage => Url.Link(routeName, new
                {
                    pageNumber = query.PageNumber - 1,
                    pageSize = query.PageSize
                }),
                ResourcePageUriType.NextPage => Url.Link(routeName, new
                {
                    pageNumber = query.PageNumber + 1,
                    pageSize = query.PageSize
                }),
                _ => Url.Link(routeName, new
                {
                    pageNumber = query.PageNumber,
                    pageSize = query.PageSize
                }),
            };
        }

    }
}
