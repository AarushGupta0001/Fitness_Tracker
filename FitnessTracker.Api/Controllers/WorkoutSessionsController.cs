using FitnessTracker.Api.Data;
using FitnessTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutSessionsController(AppDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<WorkoutSession>> CreateWorkoutSession(CreateWorkoutSessionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.SelectedMuscleGroups))
        {
            return BadRequest("SelectedMuscleGroups cannot be empty");
        }

        if (!DateTime.TryParse(request.Date, out var parsedDate))
        {
            return BadRequest("Invalid date format");
        }

        var session = new WorkoutSession
        {
            Username = request.Username,
            Date = parsedDate.Date,
            SelectedMuscleGroups = request.SelectedMuscleGroups,
            CreatedAt = DateTime.UtcNow
        };

        context.WorkoutSessions.Add(session);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWorkoutSession), new { id = session.Id }, session);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkoutSession>> GetWorkoutSession(int id)
    {
        var session = await context.WorkoutSessions.FindAsync(id);
        if (session == null)
        {
            return NotFound();
        }

        return session;
    }

    [HttpGet("date/{date}")]
    public async Task<ActionResult<IEnumerable<WorkoutSession>>> GetWorkoutSessionsByDate(string date)
    {
        if (!DateTime.TryParse(date, out var parsedDate))
        {
            return BadRequest("Invalid date format");
        }

        var sessions = await context.WorkoutSessions
            .Where(s => s.Date.Date == parsedDate.Date)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return Ok(sessions);
    }
}

public class CreateWorkoutSessionRequest
{
    public string Username { get; set; } = string.Empty;
    public string SelectedMuscleGroups { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
}
