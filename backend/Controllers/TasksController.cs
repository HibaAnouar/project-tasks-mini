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

        var project = await _db.Projects
            .Include(p => p.Tasks)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == projectId && p.User.Email == userEmail);

        if (project == null)
            return NotFound();

        var tasks = project.Tasks.Select(t => new TaskDto(
            t.Id,
            t.Title,
            t.IsCompleted
        ));

        return Ok(tasks);
    }

    // POST: api/projects/{projectId}/tasks
    [HttpPost]
    public async Task<IActionResult> CreateTask(int projectId, [FromBody] string title)
    {
        var userEmail = User.Identity!.Name!;

        var project = await _db.Projects
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == projectId && p.User.Email == userEmail);

        if (project == null)
            return NotFound();

        var task = new Models.TaskItem
        {
            Title = title,
            ProjectId = projectId
        };

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();

        var dto = new TaskDto(
            task.Id,
            task.Title,
            task.IsCompleted
        );

        return Ok(dto);
    }

    // PUT: api/projects/{projectId}/tasks/{taskId}/complete
    [HttpPut("{taskId}/complete")]
    public async Task<IActionResult> MarkTaskCompleted(int projectId, int taskId)
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
    public async Task<IActionResult> DeleteTask(int projectId, int taskId)
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
