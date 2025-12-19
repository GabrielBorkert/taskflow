using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Interfaces
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();
        Task<TaskDto?> GetTaskByIdAsync(int id);
        Task<TaskDto> CreateTaskAsync(CreateTaskDto createTaskDto);
        Task<TaskDto?> UpdateTaskAsync(int id, UpdateTaskDto updateTaskDto);
        Task<bool> DeleteTaskAsync(int id);
    }
}
