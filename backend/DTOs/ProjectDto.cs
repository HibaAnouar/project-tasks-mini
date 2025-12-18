namespace backend.DTOs;

public record ProjectDto(
    int Id,
    string Title,
    List<TaskDto> Tasks
);
