using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoWebApi.DataAccess;

namespace ToDoWebApi.Controllers
{
    [Route("todos")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly ToDoDbContext _dbContext;

        public ToDoController(ToDoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> PostTodo([FromBody] string toDoName)
        {
            if (string.IsNullOrEmpty(toDoName))
            {
                return BadRequest("Name should not be empty");
            }

            var toDoItemModel = new ToDoItemModel(toDoName);
            await _dbContext.AddAsync(toDoItemModel);
            await _dbContext.SaveChangesAsync();

            return Created($"todos/{toDoItemModel.Id}", toDoItemModel.Id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodo([FromRoute] Guid id)
        {
            ToDoItemModel toDoItemModel = await _dbContext.TodoItemModels
                                                          .FirstOrDefaultAsync(x => x.Id == id);

            if (toDoItemModel == null)
            {
                return NotFound("ToDo item not found");
            }

            return Ok(toDoItemModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetTodos([FromQuery] bool? isCompleted = null)
        {
            var toDoItemModels = _dbContext.TodoItemModels.AsQueryable();

            if (isCompleted.HasValue)
            {
                toDoItemModels = toDoItemModels.Where(x => x.Completed == isCompleted);
            }

            List<ToDoItemModel> toDoList = await toDoItemModels.ToListAsync();
            if (!toDoList.Any())
            {
                return NotFound("ToDo item not found");
            }

            return Ok(toDoItemModels);
        }

        [HttpPut("{id}/completed")]
        public async Task<IActionResult> Completed([FromRoute] Guid id)
        {
            ToDoItemModel toDoItemModel = await _dbContext.TodoItemModels
                                                          .FirstOrDefaultAsync(x => x.Id == id);


            if (toDoItemModel == null)
            {
                return NotFound("ToDo item not found");
            }

            toDoItemModel.SetCompleted();
            await _dbContext.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}/completed")]
        public async Task<IActionResult> Incomplete([FromRoute] Guid id)
        {
            ToDoItemModel toDoItemModel = await _dbContext.TodoItemModels
                                                          .FirstOrDefaultAsync(x => x.Id == id);


            if (toDoItemModel == null)
            {
                return NotFound("ToDo item not found");
            }

            toDoItemModel.SetIncomplete();
            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}