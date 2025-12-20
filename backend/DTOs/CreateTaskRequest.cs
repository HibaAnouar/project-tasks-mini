using System.ComponentModel.DataAnnotations;

namespace backend.DTOs;

public record CreateTaskRequest(
    [Required]
    string Title
);
