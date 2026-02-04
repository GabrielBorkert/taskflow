using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Enums;

namespace TaskFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet("GetAllTasks")]
        public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks()
        {
            var tasks = await _taskService.GetAllTasksAsync();
            return Ok(tasks);
        }

        [HttpGet("GetAllPriorities")]
        public async Task<ActionResult<IEnumerable<PrioritiesDto>>> GetAllPriorities()
        {
            var priorities = await _taskService.GetAllPrioritiesAsync();
            return Ok(priorities);
        }

        [HttpGet("GetTaskById/{id}")]
        public async Task<ActionResult<TaskDto>> GetTaskById(int id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);

            if (task == null)
                return NotFound(new { message = $"Task com ID {id} não encontrada" });

            return Ok(task);
        }

        [HttpPost("CreateTask")]
        public async Task<ActionResult<TaskDto>> CreateTask(CreateTaskDto createTaskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = await _taskService.CreateTaskAsync(createTaskDto);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        [HttpPut("UpdateTask/{id}")]
        public async Task<ActionResult<TaskDto>> UpdateTask(int id, [FromBody] UpdateTaskDto updateTaskDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var task = await _taskService.UpdateTaskAsync(id, updateTaskDto);

            if (task == null)
                return NotFound(new { message = $"Task com ID {id} não encontrada" });

            return Ok(task);
        }

        [HttpDelete("DeleteTask/{id}")]
        public async Task<ActionResult> DeleteTask(int id)
        {
            var result = await _taskService.DeleteTaskAsync(id);

            if (!result)
                return NotFound(new { message = $"Task com ID {id} não encontrada" });

            return NoContent();
        }
    }
}