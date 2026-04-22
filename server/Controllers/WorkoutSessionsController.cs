using FitnessTracker.Api.Data;
using FitnessTracker.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkoutSessionsController(AppDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<WorkoutSessionResponse>> CreateWorkoutSession(CreateWorkoutSessionRequest request)
    {
        var claimUsername = User.FindFirst("username")?.Value;
        var username = claimUsername ?? request.Username;

        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized("Missing user context");
        }

        if (!string.IsNullOrWhiteSpace(request.Username) && request.Username != username)
        {
            return Forbid();
        }

        if (string.IsNullOrWhiteSpace(request.SelectedMuscleGroups))
        {
            return BadRequest("SelectedMuscleGroups cannot be empty");
        }

        if (!DateTime.TryParse(request.Date, out var parsedDate))
        {
            return BadRequest("Invalid date format");
        }

        var selectedGroups = request.SelectedMuscleGroups
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(g => g.ToLowerInvariant())
            .Distinct()
            .ToList();

        if (selectedGroups.Count == 0)
        {
            return BadRequest("At least one muscle group must be selected.");
        }

        var workoutTypeCode = ResolveWorkoutTypeCode(selectedGroups);
        var workoutType = await context.WorkoutTypes.FirstOrDefaultAsync(w => w.Code == workoutTypeCode)
            ?? await context.WorkoutTypes.OrderBy(w => w.Id).FirstAsync();

        var session = new WorkoutSession
        {
            Username = username,
            Date = parsedDate.Date,
            SelectedMuscleGroups = request.SelectedMuscleGroups,
            WorkoutTypeId = workoutType.Id,
            CreatedAt = DateTime.UtcNow
        };

        context.WorkoutSessions.Add(session);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetWorkoutSession), new { id = session.Id }, new WorkoutSessionResponse
        {
            Id = session.Id,
            Username = session.Username,
            Date = session.Date,
            SelectedMuscleGroups = session.SelectedMuscleGroups,
            WorkoutType = workoutType.Name,
            WorkoutTypeCode = workoutType.Code,
            CreatedAt = session.CreatedAt
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkoutSessionResponse>> GetWorkoutSession(int id)
    {
        var session = await context.WorkoutSessions
            .Include(s => s.WorkoutType)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (session == null)
        {
            return NotFound();
        }

        return Ok(ToResponse(session));
    }

    [HttpGet("date/{date}")]
    public async Task<ActionResult<IEnumerable<WorkoutSessionResponse>>> GetWorkoutSessionsByDate(string date)
    {
        if (!DateTime.TryParse(date, out var parsedDate))
        {
            return BadRequest("Invalid date format");
        }

        var sessions = await context.WorkoutSessions
            .Include(s => s.WorkoutType)
            .Where(s => s.Date.Date == parsedDate.Date)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        return Ok(sessions.Select(ToResponse));
    }

    private static string ResolveWorkoutTypeCode(IReadOnlyCollection<string> selectedGroups)
    {
        var hasChest = selectedGroups.Contains("chest");
        var hasTriceps = selectedGroups.Contains("triceps");
        var hasBack = selectedGroups.Contains("back");
        var hasBiceps = selectedGroups.Contains("biceps");
        var hasShoulder = selectedGroups.Contains("shoulder");
        var hasLegs = selectedGroups.Contains("legs");

        if (hasBack && hasBiceps && selectedGroups.Count == 2)
        {
            return "pull";
        }

        if (hasChest && hasTriceps && !hasLegs && (!hasShoulder || selectedGroups.Count >= 3))
        {
            return "push";
        }

        if (hasLegs && selectedGroups.Count == 1)
        {
            return "lower";
        }

        if (hasLegs && hasShoulder && selectedGroups.Count == 2)
        {
            return "leg-shoulder";
        }

        if (hasLegs && selectedGroups.Count > 1)
        {
            return "lower";
        }

        if (!hasLegs)
        {
            return "upper";
        }

        return "upper";
    }

    private static WorkoutSessionResponse ToResponse(WorkoutSession session)
    {
        return new WorkoutSessionResponse
        {
            Id = session.Id,
            Username = session.Username,
            Date = session.Date,
            SelectedMuscleGroups = session.SelectedMuscleGroups,
            WorkoutType = session.WorkoutType?.Name ?? string.Empty,
            WorkoutTypeCode = session.WorkoutType?.Code ?? string.Empty,
            CreatedAt = session.CreatedAt
        };
    }
}

public class CreateWorkoutSessionRequest
{
    public string Username { get; set; } = string.Empty;
    public string SelectedMuscleGroups { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
}

public class WorkoutSessionResponse
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string SelectedMuscleGroups { get; set; } = string.Empty;
    public string WorkoutType { get; set; } = string.Empty;
    public string WorkoutTypeCode { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
