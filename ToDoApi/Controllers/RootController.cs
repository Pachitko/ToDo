using Google.Apis.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ToDoApi.Models;
namespace ToDoApi.Controllers
{
    [Route("api")]
    [ApiController]
    [AllowAnonymous]
    public class RootController : ControllerBase
    {
        private static readonly string LinksKey = nameof(GetRoot);
        private readonly IDistributedCache _distributedCache;
        private readonly ILogger<RootController> _logger;

        public RootController(IDistributedCache distributedCache, ILogger<RootController> logger)
        {
            _distributedCache = distributedCache;
            _logger = logger;
        }

        [HttpGet(Name = nameof(GetRoot))]
        [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<List<LinkDto>>> GetRoot()
        {
            var linksJson = await _distributedCache.GetStringAsync(LinksKey);
            if (!string.IsNullOrEmpty(linksJson))
            {
                _logger.LogInformation("Return links from cache");
                return JsonConvert.DeserializeObject<List<LinkDto>>(linksJson);
            }

            List<LinkDto> links = new()
            {
                new(Url.Link(nameof(GetRoot), null), "self", "GET"),
                new(Url.Link(nameof(UsersController.GetUsersAsync), null), "users", "GET"),
                new(Url.Link(nameof(UsersController.CreateUserAsync), null), "create_user", "POST")
            };
            _logger.LogInformation("Create links");

            DistributedCacheEntryOptions cacheEntryOptions = new()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                SlidingExpiration = TimeSpan.FromSeconds(10)
            };

            linksJson = JsonConvert.SerializeObject(links);
            await _distributedCache.SetStringAsync(LinksKey, linksJson, cacheEntryOptions);
            return links;
        }

        [HttpOptions()]
        public ActionResult GetOptions()
        {
            Response.Headers.Remove("Allow");
            Response.Headers.Add("Allow", "GET");
            return Ok();
        }
    }
}