using System.ComponentModel.DataAnnotations;

namespace backend.DTOs;

public record CreateTaskRequest(
    [Required]
    string Title,
    string? Description = null,
    DateTime? DueDate = null
);
