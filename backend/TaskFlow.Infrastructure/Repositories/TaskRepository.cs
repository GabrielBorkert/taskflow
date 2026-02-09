using Microsoft.EntityFrameworkCore;
using TaskFlow.Domain.Entities;
using TaskFlow.Infrastructure.Data;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TaskEntity>> GetAllAsync()
        {
            return await _context.Tasks
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<PrioritiesEntity>> GetAllPrioritiesAsync()
        {
            return await _context.Priorities
                .OrderBy(p => p.DisplayOrder)
                .ToListAsync();
        }

        public async Task<TaskEntity?> GetByIdAsync(int id)
        {
            return await _context.Tasks.FindAsync(id);
        }

        public async Task<TaskEntity> CreateAsync(TaskEntity task)
        {
            task.CreatedAt = DateTime.Now;
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskEntity> UpdateAsync(TaskEntity task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}