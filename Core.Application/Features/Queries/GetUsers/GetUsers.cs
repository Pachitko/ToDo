using Microsoft.AspNetCore.Identity;
using Core.Application.Responses;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;
using FluentValidation;

namespace Core.Application.Features.Queries.GetUsers
{
    public partial class GetUsers
    {
        public class Query : PageParameters, IRequestWrapper<PagedList<AppUser>>
        {
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            const int _maxPageSize = 20;

            public QueryValidator()
            {
                RuleFor(x => x.PageNumber)
                    .NotNull()
                    .GreaterThan(0);

                RuleFor(x => x.PageSize)
                    .NotNull()
                    .InclusiveBetween(1, _maxPageSize);
            }
        }

        public class QueryHandler : IHandlerWrapper<Query, PagedList<AppUser>>
        {
            private readonly UserManager<AppUser> _userManager;

            public QueryHandler(UserManager<AppUser> userManager)
            {
                _userManager = userManager;
            }

            public async Task<Response<PagedList<AppUser>>> Handle(Query request, CancellationToken cancellationToken)
            {
                return ResponseResult.Ok(await PagedList<AppUser>.CreateAsync(_userManager.Users, 
                    request.PageNumber.GetValueOrDefault(), request.PageSize.GetValueOrDefault(), cancellationToken));
            }
        }
    }
}