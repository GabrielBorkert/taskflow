using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _taskRepository.GetAllAsync();
            return tasks.Select(t => MapToDto(t));
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int id)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            return task != null ? MapToDto(task) : null;
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto)
        {
            var task = new TaskEntity
            {
                Title = createTaskDto.Title,
                Description = createTaskDto.Description,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            var createdTask = await _taskRepository.CreateAsync(task);
            return MapToDto(createdTask);
        }

        public async Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task == null)
                return null;

            task.Title = updateTaskDto.Title;
            task.Description = updateTaskDto.Description;
            task.IsCompleted = updateTaskDto.IsCompleted;

            if (updateTaskDto.IsCompleted && task.CompletedAt == null)
            {
                task.CompletedAt = DateTime.UtcNow;
            }
            else if (!updateTaskDto.IsCompleted)
            {
                task.CompletedAt = null;
            }

            var updatedTask = await _taskRepository.UpdateAsync(task);
            return MapToDto(updatedTask);
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            return await _taskRepository.DeleteAsync(id);
        }

        // Método auxiliar para mapear Entity → DTO
        private static TaskDto MapToDto(TaskEntity task)
        {
            return new TaskDto
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt
            };
        }
    }
}