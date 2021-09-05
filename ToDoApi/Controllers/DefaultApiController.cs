using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ToDoApi.Controllers
{
    [Route("api")]
    [AllowAnonymous]
    public class DefaultApiController : ControllerBase
    {
        private const string _baseUrl = "https://localhost:5001/api";

        private readonly Dictionary<string, string> _links = new()
        {
            ["users"] = $"{_baseUrl}/users",
            ["taskLists"] = $"{_baseUrl}/taskLists",
            ["tasks"] = $"{_baseUrl}/taskLists/{{taskListId}}",
        };

        [HttpGet]
        public ActionResult<Dictionary<string, string>> Get()
        {
            return _links;
        }
    }
}