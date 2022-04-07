using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoApi.Models;

namespace ToDoApi.Controllers
{
    [Route("api")]
    [ApiController]
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

        [HttpOptions()]
        public ActionResult GetOptions()
        {
            Response.Headers.Remove("Allow");
            Response.Headers.Add("Allow", "GET");
            return Ok();
        }
    }
}