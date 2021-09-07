using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Core.Application.Helpers;

namespace ToDoApi.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        public override ActionResult ValidationProblem()
        {
            var options = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        protected string CreateResourcePageUri(IHasPageParametersWithOrderBy query, string routeName, ResourcePageUriType type)
        {
            return type switch
            {
                ResourcePageUriType.PreviousPage => Url.Link(routeName, new
                {
                    pageNumber = query.PageNumber - 1,
                    pageSize = query.PageSize,
                    orderBy = query.OrderBy
                }),
                ResourcePageUriType.NextPage => Url.Link(routeName, new
                {
                    pageNumber = query.PageNumber + 1,
                    pageSize = query.PageSize,
                    orderBy = query.OrderBy
                }),
                _ => Url.Link(routeName, new
                {
                    pageNumber = query.PageNumber,
                    pageSize = query.PageSize,
                    orderBy = query.OrderBy
                }),
            };
        }

    }
}
