using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItems> CreateAsync(TaskItems task);
        Task<TaskItems?> GetByIdAsync(int id);
        Task<IEnumerable<TaskItems>> GetAllAsync();
        Task<TaskItems> UpdateAsync(TaskItems task);
        Task DeleteAsync(int id);
    }
}