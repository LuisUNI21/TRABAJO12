using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Application.Interfaces;
using TaskManager.Infrastructure.Persistence;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Registrar DbContext (ajusta el proveedor/connection string según tu entorno)
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<TaskDbContext>(options =>
                options.UseSqlServer(connectionString));

            // Registrar repositorios
            services.AddScoped<ITaskRepository, TaskRepository>();

            return services;
        }
    }
}