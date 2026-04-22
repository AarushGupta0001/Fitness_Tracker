using FitnessTracker.Api.Data;
using FitnessTracker.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExercisesController(AppDbContext context) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<Exercise>> CreateExercise(CreateExerciseRequest request)
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

        var dateToStore = DateTime.UtcNow.Date;
        if (!string.IsNullOrEmpty(request.Date) && DateTime.TryParse(request.Date, out var parsedDate))
        {
            dateToStore = parsedDate.Date;
        }

        var exercise = new Exercise
        {
            Name = request.Name.Trim(),
            Weight = request.Weight,
            Sets = request.Sets,
            Reps = request.Reps,
            Date = dateToStore,
            MuscleGroup = request.MuscleGroup,
            Username = username
        };

        context.Exercises.Add(exercise);
        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetExercise), new { id = exercise.Id }, exercise);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Exercise>> GetExercise(int id)
    {
        var exercise = await context.Exercises.FindAsync(id);
        if (exercise == null)
        {
            return NotFound();
        }

        return exercise;
    }

    [HttpGet("date/{date}")]
    public async Task<ActionResult<IEnumerable<Exercise>>> GetExercisesByDate(string date)
    {
        if (!DateTime.TryParse(date, out var parsedDate))
        {
            return BadRequest("Invalid date format");
        }

        var exercises = await context.Exercises
            .Where(e => e.Date.Date == parsedDate.Date)
            .OrderBy(e => e.MuscleGroup)
            .ToListAsync();

        return Ok(exercises);
    }

    [HttpGet("date/{date}/musclegroup/{muscleGroup}")]
    public async Task<ActionResult<IEnumerable<Exercise>>> GetExercisesByDateAndMuscleGroup(string date, int muscleGroup)
    {
        if (!DateTime.TryParse(date, out var parsedDate))
        {
            return BadRequest("Invalid date format");
        }

        var muscleGroupEnum = (MuscleGroup)muscleGroup;
        var exercises = await context.Exercises
            .Where(e => e.Date.Date == parsedDate.Date && e.MuscleGroup == muscleGroupEnum)
            .ToListAsync();

        return Ok(exercises);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExercise(int id)
    {
        var exercise = await context.Exercises.FindAsync(id);
        if (exercise == null)
        {
            return NotFound();
        }

        context.Exercises.Remove(exercise);
        await context.SaveChangesAsync();

        return NoContent();
    }
}

public class CreateExerciseRequest
{
    public string Name { get; set; } = string.Empty;
    public decimal Weight { get; set; }
    public int Sets { get; set; }
    public int Reps { get; set; }
    public MuscleGroup MuscleGroup { get; set; }
    public string Username { get; set; } = string.Empty;
    public string? Date { get; set; }
}
