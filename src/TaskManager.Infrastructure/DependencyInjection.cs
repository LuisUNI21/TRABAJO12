using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // Register infrastructure services here
            // e.g. services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<TaskManager.Application.Interfaces.ITaskRepository, TaskManager.Infrastructure.Repositories.TaskRepository>();

            return services;
        }
    }
}