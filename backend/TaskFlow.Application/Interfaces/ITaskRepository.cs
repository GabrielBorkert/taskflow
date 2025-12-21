using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskEntity>> GetAllAsync();
        Task<TaskEntity?> GetByIdAsync(int id);
        Task<TaskEntity> CreateAsync(TaskEntity task);
        Task<TaskEntity> UpdateAsync(TaskEntity task);
        Task<bool> DeleteAsync(int id);
    }
}
