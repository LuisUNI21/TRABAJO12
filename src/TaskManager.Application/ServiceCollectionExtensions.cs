using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;

namespace TaskManager.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // Register application layer services
            services.AddScoped<ITaskService, TaskService>();

            return services;
        }
    }
}
