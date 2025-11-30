using System;
using TaskManager.Domain;

namespace TaskManager.Domain.Entities
{
    public class TaskItems
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TaskStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        // Inicializador por defecto para evitar que EF intente insertar NULL si por alguna razón no se asigna.
        public bool IsActive { get; set; } = true;
    }
}