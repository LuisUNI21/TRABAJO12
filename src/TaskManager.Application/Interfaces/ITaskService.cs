namespace TaskManager.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TaskManager.Application.DTOs;

    public interface ITaskService
    {
        Task<TaskDto> CreateTaskAsync(TaskDto taskDto);
        Task<TaskDto?> GetTaskByIdAsync(Guid id);
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();
        Task<bool> UpdateTaskAsync(TaskDto taskDto);
        Task<bool> DeleteTaskAsync(Guid id);
    }
}