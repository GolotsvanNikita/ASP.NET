using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;

namespace TodoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private static readonly List<TodoItem> Todos = new()
        {
            new TodoItem { Id = 1, Name = "Learn C#", IsComplete = true },
            new TodoItem { Id = 2, Name = "Work in company", IsComplete = false }
        };

        [HttpGet]
        public ActionResult<List<TodoItem>> GetAll() 
        {
            return Ok(Todos);
        }
    }
}
