namespace backend.DTOs;

public record TaskDto(
    int Id,
    string Title,
    bool IsCompleted,
    string? Description = null,
    DateTime? DueDate = null
);
