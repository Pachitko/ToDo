using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoApi.Models;
using System.Linq;

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
            if (User.Identity.IsAuthenticated)
            {
                return Ok(new
                {
                    name = User.Identity.Name,
                    email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                    isAuthenticated = User.Identity.IsAuthenticated,
                }); ;
            }
            else
            {
                return NotFound();
            }

        }

        [HttpOptions("me")]
        public ActionResult GetMeOptions()
        {
            Response.Headers.Remove("Allow");
            Response.Headers.Add("Allow", "GET");
            return Ok();
        }
    }
}