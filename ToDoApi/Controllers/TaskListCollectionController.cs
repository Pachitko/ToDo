using Core.Application.Features.Commands.CreateToDoList;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ToDoApi.Controllers
{
    [ApiController]
    [Route("api/usercollections")]
    public class TaskListCollectionController : ControllerBase
    {
        public async Task<IActionResult> CreateToDoListCollection(IEnumerable<CreateToDoLists.Command> command)
        {
            return Ok();
        } 
    }
}