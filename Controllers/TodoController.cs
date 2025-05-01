using Microsoft.AspNetCore.Mvc;
using TodoApi.IRepository;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodosController : ControllerBase
    {
        private readonly ITodoRepository _todoRepository;

        public TodosController(ITodoRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _todoRepository.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var todo = await _todoRepository.GetByIdAsync(id);
            if (todo == null) return NotFound();
            return Ok(todo);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Todo todo)
        {
            var created = await _todoRepository.CreateAsync(todo);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Todo todo)
        {
            if (!await _todoRepository.UpdateAsync(id, todo)) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (!await _todoRepository.DeleteAsync(id)) return NotFound();
            return NoContent();
        }
    }

}
