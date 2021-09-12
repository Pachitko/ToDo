using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [Route("api")]
    [AllowAnonymous]
    public class RootController : ControllerBase
    {
        [HttpGet(Name = nameof(GetRoot))]
        public ActionResult<List<LinkDto>> GetRoot()
        {
            List<LinkDto> links = new();

            links.Add(new(Url.Link(nameof(GetRoot), null), "self", "GET"));
            links.Add(new(Url.Link(nameof(UsersController.GetUsersAsync), null), "users", "GET"));
            links.Add(new(Url.Link(nameof(UsersController.CreateUserAsync), null), "create_user", "POST"));

            return links;
        }

        [HttpGet("me")]
        public ActionResult GetMe()
        {
            return Ok(new
            {
                User.Identity.Name,
                User.Identity.AuthenticationType,
                User.Identity.IsAuthenticated,
                Calims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }

    }
}