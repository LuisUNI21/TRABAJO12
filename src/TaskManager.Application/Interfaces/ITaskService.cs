namespace TaskManager.Application.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using TaskManager.Application.DTOs;

    public interface ITaskService
    {
        Task<TaskDto> CreateTaskAsync(TaskDto taskDto);
        Task<TaskDto?> GetTaskByIdAsync(int id);
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();
        Task<bool> UpdateTaskAsync(TaskDto taskDto);
        Task<bool> DeleteTaskAsync(int id);
    }
}