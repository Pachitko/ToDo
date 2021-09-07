using Microsoft.AspNetCore.Identity;
using Core.Application.Extensions;
using Core.Application.Responses;
using Core.Application.Services;
using Core.Application.Helpers;
using System.Threading.Tasks;
using Core.Domain.Entities;
using System.Threading;
using FluentValidation;
using System;

namespace Core.Application.Features.Queries.GetUsers
{
    public partial class GetUsers
    {
        public class Query : IHasPageParametersWithOrderBy, IRequestWrapper<PagedList<AppUser>>
        {
            public int PageNumber { get; set; } = 1;
            public int PageSize { get; set; } = 10;
            public string OrderBy { get; set; } = "ID";
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
            private readonly IPropertyMappingService _propertyMappingService;

            public QueryHandler(UserManager<AppUser> userManager, IPropertyMappingService propertyMappingService)
            {
                _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
                _propertyMappingService = propertyMappingService ?? throw new ArgumentNullException(nameof(propertyMappingService));
            }

            public async Task<Response<PagedList<AppUser>>> Handle(Query request, CancellationToken cancellationToken)
            {
                var collection = _userManager.Users;

                if (!string.IsNullOrWhiteSpace(request.OrderBy))
                {
                    var mappingDictionary = _propertyMappingService.GetPropertyMapping<AppUser>();
                    collection = collection.ApplySort(request.OrderBy, mappingDictionary);
                }

                return ResponseResult.Ok(await PagedList<AppUser>.CreateAsync(
                    collection, 
                    request.PageNumber, 
                    request.PageSize, 
                    cancellationToken));
            }
        }
    }
}