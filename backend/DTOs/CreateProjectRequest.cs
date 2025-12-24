using System.ComponentModel.DataAnnotations;

namespace backend.DTOs;

public record CreateProjectRequest(
    [Required]
    string Title,
    string? Description
);
