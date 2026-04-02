using FitnessTracker.Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutLogsController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkoutLogListItemResponse>>> GetWorkoutLogs([FromQuery] string? username)
    {
        var sessionsQuery = context.WorkoutSessions
            .Include(s => s.WorkoutType)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(username))
        {
            sessionsQuery = sessionsQuery.Where(s => s.Username == username);
        }

        var sessions = await sessionsQuery
            .OrderByDescending(s => s.Date)
            .ThenByDescending(s => s.CreatedAt)
            .ToListAsync();

        var sessionIds = sessions.Select(s => s.Id).ToList();

        var logs = await context.ExerciseLogs
            .Include(l => l.ExerciseTemplate)
                .ThenInclude(t => t!.ExerciseObjectType)
            .Where(l => l.WorkoutSessionId.HasValue && sessionIds.Contains(l.WorkoutSessionId.Value))
            .OrderBy(l => l.CreatedAt)
            .ToListAsync();

        var sessionLogs = sessions.Select(session =>
        {
            var exerciseGroups = logs
                .Where(log => log.WorkoutSessionId == session.Id)
                .GroupBy(log => new
                {
                    log.ExerciseTemplateId,
                    log.ExerciseName,
                    log.MuscleGroup,
                    ObjectName = log.ExerciseTemplate?.ExerciseObjectType?.Name ?? string.Empty
                })
                .Select(group =>
                {
                    var orderedSets = group.OrderBy(g => g.SetNumber).ToList();
                    var isSkipped = orderedSets.Any(s => s.IsSkipped);

                    return new WorkoutLogExerciseResponse
                    {
                        ExerciseTemplateId = group.Key.ExerciseTemplateId ?? 0,
                        ExerciseName = group.Key.ExerciseName,
                        MuscleGroup = group.Key.MuscleGroup.ToString(),
                        Object = group.Key.ObjectName,
                        IsSkipped = isSkipped,
                        SetsCount = isSkipped ? 0 : orderedSets.Count,
                        Sets = orderedSets.Select(s => new WorkoutLogSetResponse
                        {
                            SetNumber = s.SetNumber,
                            Weight = s.Weight
                        }).ToList()
                    };
                })
                .OrderBy(e => e.MuscleGroup)
                .ThenBy(e => e.ExerciseName)
                .ToList();

            return new WorkoutLogListItemResponse
            {
                WorkoutSessionId = session.Id,
                Date = session.Date,
                Workout = session.WorkoutType?.Name ?? string.Empty,
                Exercises = exerciseGroups
            };
        }).ToList();

        return Ok(sessionLogs);
    }
}

public class WorkoutLogListItemResponse
{
    public int WorkoutSessionId { get; set; }
    public DateTime Date { get; set; }
    public string Workout { get; set; } = string.Empty;
    public List<WorkoutLogExerciseResponse> Exercises { get; set; } = [];
}

public class WorkoutLogExerciseResponse
{
    public int ExerciseTemplateId { get; set; }
    public string ExerciseName { get; set; } = string.Empty;
    public string MuscleGroup { get; set; } = string.Empty;
    public string Object { get; set; } = string.Empty;
    public int SetsCount { get; set; }
    public bool IsSkipped { get; set; }
    public List<WorkoutLogSetResponse> Sets { get; set; } = [];
}

public class WorkoutLogSetResponse
{
    public int SetNumber { get; set; }
    public decimal? Weight { get; set; }
}
