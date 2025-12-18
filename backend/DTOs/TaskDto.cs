namespace backend.DTOs;

public record TaskDto(
    int Id,
    string Title,
    bool IsCompleted
);
