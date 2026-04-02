using FitnessTracker.Api.Data;
using FitnessTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExerciseLogsController(AppDbContext context) : ControllerBase
{
    [HttpPost("bulk")]
    public async Task<ActionResult<BulkExerciseLogResponse>> CreateBulkLogs(CreateBulkExerciseLogRequest request)
    {
        if (!DateTime.TryParse(request.Date, out var parsedDate))
        {
            return BadRequest("Invalid date format");
        }

        if (!Enum.TryParse<MuscleGroup>(request.MuscleGroup, true, out var parsedMuscleGroup))
        {
            return BadRequest("Invalid muscle group");
        }

        if (request.Exercises == null || request.Exercises.Count == 0)
        {
            return BadRequest("At least one exercise is required");
        }

        var logs = new List<ExerciseLog>();

        foreach (var exercise in request.Exercises)
        {
            if (string.IsNullOrWhiteSpace(exercise.ExerciseName))
            {
                return BadRequest("Exercise name cannot be empty");
            }

            if (exercise.Weights == null || exercise.Weights.Count == 0)
            {
                return BadRequest($"No weights provided for exercise {exercise.ExerciseName}");
            }

            if (exercise.Weights.Count > 5)
            {
                return BadRequest($"Exercise {exercise.ExerciseName} has more than 5 sets");
            }

            for (var i = 0; i < exercise.Weights.Count; i++)
            {
                logs.Add(new ExerciseLog
                {
                    Username = request.Username,
                    Date = parsedDate.Date,
                    MuscleGroup = parsedMuscleGroup,
                    ExerciseName = exercise.ExerciseName.Trim(),
                    SetNumber = i + 1,
                    Weight = exercise.Weights[i],
                    CreatedAt = DateTime.UtcNow
                });
            }
        }

        await context.ExerciseLogs.AddRangeAsync(logs);
        await context.SaveChangesAsync();

        return Ok(new BulkExerciseLogResponse
        {
            CreatedCount = logs.Count,
            Message = "Exercise logs saved successfully"
        });
    }

    [HttpGet("date/{date}")]
    public async Task<ActionResult<IEnumerable<ExerciseLog>>> GetLogsByDate(string date)
    {
        if (!DateTime.TryParse(date, out var parsedDate))
        {
            return BadRequest("Invalid date format");
        }

        var logs = await context.ExerciseLogs
            .Where(l => l.Date.Date == parsedDate.Date)
            .OrderBy(l => l.MuscleGroup)
            .ThenBy(l => l.ExerciseName)
            .ThenBy(l => l.SetNumber)
            .ToListAsync();

        return Ok(logs);
    }
}

public class CreateBulkExerciseLogRequest
{
    public string Username { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string MuscleGroup { get; set; } = string.Empty;
    public List<CreateExerciseLogItem> Exercises { get; set; } = [];
}

public class CreateExerciseLogItem
{
    public string ExerciseName { get; set; } = string.Empty;
    public List<decimal> Weights { get; set; } = [];
}

public class BulkExerciseLogResponse
{
    public int CreatedCount { get; set; }
    public string Message { get; set; } = string.Empty;
}
