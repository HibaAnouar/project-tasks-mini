namespace backend.DTOs;

public record ProjectDto(
    int Id,
    string Title,
    string? Description,
    List<TaskDto> Tasks
);
