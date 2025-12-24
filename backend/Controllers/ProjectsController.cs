using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend.DTOs;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _db;

    public ProjectsController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/projects
    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var userEmail = User.Identity!.Name!;
        var user = await _db.Users
            .Include(u => u.Projects)
                .ThenInclude(p => p.Tasks)
            .FirstOrDefaultAsync(u => u.Email == userEmail);

        if (user == null) return Unauthorized();

        var result = user.Projects.Select(p => new ProjectDto(
            p.Id,
            p.Title,
            p.Description, 
            p.Tasks.Select(t => new TaskDto(t.Id, t.Title, t.IsCompleted)).ToList()
        ));

        return Ok(result);
    }

    // POST: api/projects
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        var userEmail = User.Identity!.Name!;
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
        if (user == null) return Unauthorized();

        var project = new Project
        {
            Title = request.Title,
            Description = request.Description, 
            UserId = user.Id
        };

        _db.Projects.Add(project);
        await _db.SaveChangesAsync();

        return Ok(new ProjectDto(
            project.Id,
            project.Title,
            project.Description, 
            new List<TaskDto>()
        ));
    }

    // GET: api/projects/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProjectById(int id)
    {
        var userEmail = User.Identity!.Name!;
        var project = await _db.Projects
            .Include(p => p.Tasks)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id && p.User.Email == userEmail);

        if (project == null) return NotFound();

        var dto = new ProjectDto(
            project.Id,
            project.Title,
            project.Description,
            project.Tasks.Select(t => new TaskDto(t.Id, t.Title, t.IsCompleted)).ToList()
        );

        return Ok(dto);
    }

    // GET: api/projects/{id}/progress
    [HttpGet("{id}/progress")]
    public async Task<IActionResult> GetProjectProgress(int id)
    {
        var userEmail = User.Identity!.Name!;
        var project = await _db.Projects
            .Include(p => p.Tasks)
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id && p.User.Email == userEmail);

        if (project == null) return NotFound();

        var total = project.Tasks.Count;
        var completed = project.Tasks.Count(t => t.IsCompleted);
        var progress = total == 0 ? 0 : (completed * 100 / total);

        return Ok(new
        {
            totalTasks = total,
            completedTasks = completed,
            progressPercentage = progress
        });
    }

    // DELETE: api/projects/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var userEmail = User.Identity!.Name!;
        var project = await _db.Projects
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.Id == id && p.User.Email == userEmail);

        if (project == null) return NotFound();

        _db.Projects.Remove(project);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}
