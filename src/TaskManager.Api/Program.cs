using Microsoft.EntityFrameworkCore; // Añadido para Database.Migrate/EnsureCreated
using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using TaskManager.Application.Interfaces;
using TaskManager.Application.Services;
using TaskManager.Infrastructure;
using TaskManager.Infrastructure.Persistence; // añadido

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
            var errors = context.ModelState
                .ToList()
                .Where(kvp => kvp.Value.Errors.Count > 0)
                .Select(kvp => new { Field = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage) })
                .ToList();
            logger.LogWarning("Model validation failed: {@Errors}", errors);
            return new BadRequestObjectResult(context.ModelState);
        };
    });

// Registrar servicios de la capa Application / Infrastructure
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Swagger
builder.Services.AddAuthorization();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Corspolicy",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
});

var app = builder.Build();

// Aplicar migraciones al arrancar (safe, con logging)
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        logger.LogInformation("Applying database migrations (if any)...");
        var dbContext = scope.ServiceProvider.GetRequiredService<TaskDbContext>();

        try
        {
            // Intento preferente: aplicar migraciones existentes
            dbContext.Database.Migrate();
            logger.LogInformation("Database migrations applied.");
        }
        catch (Exception migrateEx)
        {
            // Fallback para entornos de desarrollo donde no hay migraciones creadas
            logger.LogWarning(migrateEx, "Migrate() failed. Intentando EnsureCreated() como fallback.");
            try
            {
                if (dbContext.Database.EnsureCreated())
                {
                    logger.LogInformation("Database schema created with EnsureCreated().");
                }
                else
                {
                    logger.LogWarning("EnsureCreated() did not create the schema (maybe already exists).");
                }
            }
            catch (Exception ensureEx)
            {
                logger.LogError(ensureEx, "EnsureCreated() también falló.");
                throw;
            }
        }
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating or connecting to the database. Check your connection string and that the SQL Server instance is reachable.");
        throw;
    }
}

// Enable developer exception page in Development, otherwise configure a simple JSON exception handler
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";
            var exceptionHandler = context.Features.Get<IExceptionHandlerFeature>();
            var error = exceptionHandler?.Error;
            var result = JsonSerializer.Serialize(new { error = error?.Message ?? "An error occurred." });
            await context.Response.WriteAsync(result);
        });
    });
    app.UseHsts();
}

// Swagger UI (exposed at application root)
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApi v1");
    c.RoutePrefix = string.Empty;
});

app.UseRouting();
app.UseCors("Corspolicy");
app.UseAuthentication();
app.UseAuthorization();

// Use endpoint mapping for controllers
app.MapControllers();

app.Run();
