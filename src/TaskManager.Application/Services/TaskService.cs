using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<TaskDto> CreateTaskAsync(TaskDto taskDto)
        {
            var taskItem = new TaskItems
            {
                Title = taskDto.Title,
                Description = taskDto.Description,
                Status = taskDto.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var created = await _repository.CreateAsync(taskItem);

            return new TaskDto
            {
                Id = created.Id,
                Title = created.Title,
                Description = created.Description,
                Status = created.Status,
                CreatedAt = created.CreatedAt,
                UpdatedAt = created.UpdatedAt,
                IsActive = created.IsActive
            };
        }

        public async Task<TaskDto?> GetTaskByIdAsync(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null || !item.IsActive) return null;

            return new TaskDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Status = item.Status,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                IsActive = item.IsActive
            };
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            var items = await _repository.GetAllAsync();
            return items.Select(item => new TaskDto
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Status = item.Status,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                IsActive = item.IsActive
            }).ToList();
        }

        public async Task<bool> UpdateTaskAsync(TaskDto taskDto)
        {
            var existing = await _repository.GetByIdAsync(taskDto.Id);
            if (existing == null || !existing.IsActive) return false;

            existing.Title = taskDto.Title;
            existing.Description = taskDto.Description;
            existing.Status = taskDto.Status;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);
            return true;
        }

        public async Task<bool> DeleteTaskAsync(int id)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null || !existing.IsActive) return false;

            await _repository.DeleteAsync(id);
            return true;
        }
    }
}