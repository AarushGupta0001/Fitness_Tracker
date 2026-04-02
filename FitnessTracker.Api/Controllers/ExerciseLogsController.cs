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

        var session = await context.WorkoutSessions
            .FirstOrDefaultAsync(s => s.Id == request.WorkoutSessionId);

        if (session == null)
        {
            return BadRequest("Workout session not found.");
        }

        if (request.Exercises == null || request.Exercises.Count == 0)
        {
            return BadRequest("At least one exercise is required");
        }

        var templates = await context.ExerciseTemplates
            .Include(t => t.ExerciseObjectType)
            .ToDictionaryAsync(t => t.Id);

        var dumbelWeights = await context.WeightDb.OrderBy(w => w.DisplayOrder).Select(w => w.Value).ToListAsync();
        var barWeights = await context.WeightBar.OrderBy(w => w.DisplayOrder).Select(w => w.Value).ToListAsync();
        var machineWeights = await context.WeightMachine.OrderBy(w => w.DisplayOrder).Select(w => w.Value).ToListAsync();

        var logs = new List<ExerciseLog>();

        foreach (var exercise in request.Exercises)
        {
            if (!templates.TryGetValue(exercise.ExerciseTemplateId, out var template))
            {
                return BadRequest($"Exercise template not found for id {exercise.ExerciseTemplateId}");
            }

            if (exercise.IsSkipped)
            {
                logs.Add(new ExerciseLog
                {
                    WorkoutSessionId = session.Id,
                    ExerciseTemplateId = template.Id,
                    Username = request.Username,
                    Date = parsedDate.Date,
                    MuscleGroup = template.MuscleGroup,
                    ExerciseName = template.Name,
                    SetNumber = 0,
                    Weight = null,
                    IsSkipped = true,
                    CreatedAt = DateTime.UtcNow
                });

                continue;
            }

            if (exercise.Weights == null || exercise.Weights.Count == 0)
            {
                return BadRequest($"No weights provided for exercise {template.Name}");
            }

            if (exercise.Weights.Count > 4)
            {
                return BadRequest($"Exercise {template.Name} has more than 4 sets");
            }

            var objectName = template.ExerciseObjectType?.Name?.ToLowerInvariant() ?? string.Empty;
            var allowedWeights = objectName switch
            {
                "dumbel" => dumbelWeights,
                "bar" => barWeights,
                "machine" => machineWeights,
                _ => []
            };

            if (allowedWeights.Count == 0)
            {
                return BadRequest($"No weight table found for exercise object '{objectName}'.");
            }

            if (exercise.Weights.Any(weight => !allowedWeights.Contains(weight)))
            {
                return BadRequest($"Exercise {template.Name} contains invalid weight values for object {objectName}.");
            }

            for (var i = 0; i < exercise.Weights.Count; i++)
            {
                logs.Add(new ExerciseLog
                {
                    WorkoutSessionId = session.Id,
                    ExerciseTemplateId = template.Id,
                    Username = request.Username,
                    Date = parsedDate.Date,
                    MuscleGroup = template.MuscleGroup,
                    ExerciseName = template.Name,
                    SetNumber = i + 1,
                    Weight = exercise.Weights[i],
                    IsSkipped = false,
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
    public int WorkoutSessionId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public List<CreateExerciseLogItem> Exercises { get; set; } = [];
}

public class CreateExerciseLogItem
{
    public int ExerciseTemplateId { get; set; }
    public bool IsSkipped { get; set; }
    public List<decimal> Weights { get; set; } = [];
}

public class BulkExerciseLogResponse
{
    public int CreatedCount { get; set; }
    public string Message { get; set; } = string.Empty;
}
