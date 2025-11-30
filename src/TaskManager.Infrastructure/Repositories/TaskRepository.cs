using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskDbContext _context;

        public TaskRepository(TaskDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItems> CreateAsync(TaskItems task)
        {
            await _context.TaskItems.AddAsync(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task<TaskItems?> GetByIdAsync(int id)
        {
            return await _context.TaskItems.FindAsync(id);
        }

        public async Task<IEnumerable<TaskItems>> GetAllAsync()
        {
            return await _context.TaskItems.Where(t => t.IsActive).ToListAsync();
        }

        public async Task<TaskItems> UpdateAsync(TaskItems task)
        {
            _context.TaskItems.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task DeleteAsync(int id)
        {
            var task = await GetByIdAsync(id);
            if (task != null)
            {
                task.IsActive = false;
                await UpdateAsync(task);
            }
        }
    }
}