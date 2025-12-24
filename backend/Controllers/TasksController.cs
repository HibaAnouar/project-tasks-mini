using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend.DTOs;

namespace backend.Controllers;

[ApiController]
[Route("api/projects/{projectId}/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _db;

    public TasksController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/projects/{projectId}/tasks
    [HttpGet]
    public async Task<IActionResult> GetTasks(int projectId)
    {
        var userEmail = User.Identity!.Name!;

        var tasks = await _db.Tasks
            .Where(t =>
                t.ProjectId == projectId &&
                t.Project.User.Email == userEmail)
            .Select(t => new TaskDto(
                t.Id,
                t.Title,
                t.IsCompleted,
                t.Description,
                t.DueDate
            ))
            .ToListAsync();

        return Ok(tasks);
    }

    // POST: api/projects/{projectId}/tasks
    [HttpPost]
    public async Task<IActionResult> CreateTask(
        int projectId,
        [FromBody] CreateTaskRequest request)
    {
        if (request == null || string.IsNullOrWhiteSpace(request.Title))
            return BadRequest(new { message = "Task title is required" });

        var userEmail = User.Identity!.Name!;

        var projectExists = await _db.Projects.AnyAsync(p =>
            p.Id == projectId &&
            p.User.Email == userEmail);

        if (!projectExists)
            return NotFound();

        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,  
            DueDate = request.DueDate,          
            ProjectId = projectId
        };

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();

        var dto = new TaskDto(
            task.Id,
            task.Title,
            task.IsCompleted,
            task.Description,
            task.DueDate
        );

        return CreatedAtAction(
            nameof(GetTasks),
            new { projectId },
            dto
        );
    }

    // PUT: api/projects/{projectId}/tasks/{taskId}/complete
    [HttpPut("{taskId}/complete")]
    public async Task<IActionResult> MarkTaskCompleted(
        int projectId,
        int taskId)
    {
        var userEmail = User.Identity!.Name!;

        var task = await _db.Tasks
            .Include(t => t.Project)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(t =>
                t.Id == taskId &&
                t.ProjectId == projectId &&
                t.Project.User.Email == userEmail);

        if (task == null)
            return NotFound();

        task.IsCompleted = true;
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/projects/{projectId}/tasks/{taskId}
    [HttpDelete("{taskId}")]
    public async Task<IActionResult> DeleteTask(
        int projectId,
        int taskId)
    {
        var userEmail = User.Identity!.Name!;

        var task = await _db.Tasks
            .Include(t => t.Project)
                .ThenInclude(p => p.User)
            .FirstOrDefaultAsync(t =>
                t.Id == taskId &&
                t.ProjectId == projectId &&
                t.Project.User.Email == userEmail);

        if (task == null)
            return NotFound();

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
