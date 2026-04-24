using FitnessTracker.Api.Data;
using FitnessTracker.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InsightsController(AppDbContext context) : ControllerBase
{
    [HttpGet("progressive-overload")]
    public async Task<ActionResult<ProgressiveOverloadResponse>> GetProgressiveOverload(
        [FromQuery] string muscleGroup,
        [FromQuery] string? username)
    {
        var effectiveUsername = ResolveUsername(username);
        if (effectiveUsername == null)
        {
            return Unauthorized("Missing user context");
        }

        if (!Enum.TryParse<MuscleGroup>(muscleGroup, true, out var parsedMuscleGroup))
        {
            return BadRequest("Invalid muscleGroup value.");
        }

        var sessionData = await context.ExerciseLogs
            .Where(l =>
                l.Username == effectiveUsername &&
                l.MuscleGroup == parsedMuscleGroup &&
                !l.IsSkipped &&
                l.Weight.HasValue &&
                l.WorkoutSessionId.HasValue)
            .GroupBy(l => l.WorkoutSessionId)
            .Select(group => new SessionWeightPoint
            {
                WorkoutSessionId = group.Key ?? 0,
                Date = group.Max(x => x.Date),
                SessionTopWeight = group.Max(x => x.Weight ?? 0m),
                SessionAverageWeight = group.Average(x => x.Weight ?? 0m),
                SessionVolume = group.Sum(x => x.Weight ?? 0m),
                SetsCount = group.Count()
            })
            .OrderByDescending(s => s.Date)
            .Take(5)
            .ToListAsync();

        var ordered = sessionData
            .OrderBy(s => s.Date)
            .ThenBy(s => s.WorkoutSessionId)
            .ToList();

        if (ordered.Count == 0)
        {
            return Ok(new ProgressiveOverloadResponse
            {
                MuscleGroup = parsedMuscleGroup.ToString(),
                RecommendationType = "not-enough-data",
                Recommendation = "No workout history for this muscle group yet.",
                RecommendedIncrement = null,
                Trend = []
            });
        }

        var latest = ordered[^1];
        var previous = ordered.Count >= 2 ? ordered[^2] : null;

        var first = ordered.First().SessionTopWeight;
        var max = ordered.Max(o => o.SessionTopWeight);
        var min = ordered.Min(o => o.SessionTopWeight);
        var rangePercent = max == 0 ? 0 : (max - min) / max;

        var plateau = ordered.Count >= 3 && rangePercent <= 0.03m;
        var recentlyIncreased = previous != null && latest.SessionTopWeight > previous.SessionTopWeight;
        var dropped = previous != null && latest.SessionTopWeight + 0.01m < previous.SessionTopWeight;

        var mostUsedObject = await context.ExerciseLogs
            .Where(l =>
                l.Username == effectiveUsername &&
                l.MuscleGroup == parsedMuscleGroup &&
                l.WorkoutSessionId == latest.WorkoutSessionId)
            .Join(context.ExerciseTemplates,
                log => log.ExerciseTemplateId,
                template => template.Id,
                (log, template) => template)
            .Join(context.ExerciseObjectTypes,
                template => template.ExerciseObjectTypeId,
                obj => obj.Id,
                (template, obj) => obj.Name)
            .GroupBy(name => name)
            .OrderByDescending(group => group.Count())
            .Select(group => group.Key)
            .FirstOrDefaultAsync();

        var increment = mostUsedObject != null && mostUsedObject.Equals("dumbel", StringComparison.OrdinalIgnoreCase)
            ? 2.5m
            : 5m;

        string recommendationType;
        string recommendation;

        if (ordered.Count == 1)
        {
            recommendationType = "baseline";
            recommendation = "Baseline session recorded. Keep the same working weights for one more session, then reassess overload.";
        }
        else if (plateau)
        {
            recommendationType = "increase";
            recommendation = $"You have plateaued for this muscle group. Try adding +{increment:0.##} in your next similar session for your main lift.";
        }
        else if (recentlyIncreased)
        {
            recommendationType = "hold";
            recommendation = "You progressed in your latest session. Hold this weight for 1-2 sessions to consolidate before overloading again.";
        }
        else if (dropped)
        {
            recommendationType = "recover";
            recommendation = $"Latest performance dipped. This may be fatigue related. Use a controlled session near last working weight and a small +{increment:0.##} target only if form feels strong.";
        }
        else
        {
            recommendationType = "steady";
            recommendation = "Progress is stable. Keep execution quality high and consider a small overload when the next session feels smooth.";
        }

        var insight = new ProgressiveOverloadInsight
        {
            Username = effectiveUsername,
            MuscleGroup = parsedMuscleGroup,
            RecommendationType = recommendationType,
            Recommendation = recommendation,
            RecommendedIncrement = recommendationType == "increase" ? increment : null,
            SessionIdsCsv = string.Join(',', ordered.Select(o => o.WorkoutSessionId)),
            CreatedAt = DateTime.UtcNow
        };

        context.ProgressiveOverloadInsights.Add(insight);
        await context.SaveChangesAsync();

        return Ok(new ProgressiveOverloadResponse
        {
            MuscleGroup = parsedMuscleGroup.ToString(),
            RecommendationType = recommendationType,
            Recommendation = recommendation,
            RecommendedIncrement = insight.RecommendedIncrement,
            Trend = ordered.Select(item => new ProgressivePoint
            {
                WorkoutSessionId = item.WorkoutSessionId,
                Date = item.Date,
                TopWeight = item.SessionTopWeight,
                AverageWeight = item.SessionAverageWeight,
                Volume = item.SessionVolume,
                Sets = item.SetsCount
            }).ToList()
        });
    }

    [HttpGet("weekly-performance")]
    public async Task<ActionResult<WeeklyPerformanceResponse>> GetWeeklyPerformance(
        [FromQuery] string? weekStart,
        [FromQuery] string? username)
    {
        var effectiveUsername = ResolveUsername(username);
        if (effectiveUsername == null)
        {
            return Unauthorized("Missing user context");
        }

        var startDate = ResolveWeekStart(weekStart);
        var endDate = startDate.AddDays(6);
        var previousStart = startDate.AddDays(-7);
        var previousEnd = startDate.AddDays(-1);

        var currentSessions = await context.WorkoutSessions
            .Where(s => s.Username == effectiveUsername && s.Date >= startDate && s.Date <= endDate)
            .ToListAsync();

        var previousSessions = await context.WorkoutSessions
            .Where(s => s.Username == effectiveUsername && s.Date >= previousStart && s.Date <= previousEnd)
            .ToListAsync();

        var sessionIds = currentSessions.Select(s => s.Id).ToList();
        var previousSessionIds = previousSessions.Select(s => s.Id).ToList();

        var currentLogs = await context.ExerciseLogs
            .Where(l => sessionIds.Contains(l.WorkoutSessionId ?? 0) && !l.IsSkipped && l.Weight.HasValue)
            .ToListAsync();

        var previousLogs = await context.ExerciseLogs
            .Where(l => previousSessionIds.Contains(l.WorkoutSessionId ?? 0) && !l.IsSkipped && l.Weight.HasValue)
            .ToListAsync();

        var prCount = await context.PersonalRecords
            .CountAsync(pr => pr.Username == effectiveUsername && pr.Date >= startDate && pr.Date <= endDate);

        var muscleFrequency = currentSessions
            .SelectMany(session => session.SelectedMuscleGroups
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(group => group.Trim()))
            .GroupBy(group => group, StringComparer.OrdinalIgnoreCase)
            .Select(group => new MuscleFrequencyItem
            {
                MuscleGroup = group.Key,
                Sessions = group.Count()
            })
            .OrderByDescending(item => item.Sessions)
            .ToList();

        var ineffective = currentLogs
            .GroupBy(log => log.MuscleGroup)
            .Select(group => new
            {
                Muscle = group.Key,
                Avg = group.Average(x => x.Weight ?? 0m)
            })
            .Select(current =>
            {
                var previousAvg = previousLogs
                    .Where(log => log.MuscleGroup == current.Muscle)
                    .Select(log => log.Weight ?? 0m)
                    .DefaultIfEmpty(0m)
                    .Average();

                return new
                {
                    current.Muscle,
                    Current = current.Avg,
                    Previous = previousAvg,
                    Delta = current.Avg - previousAvg
                };
            })
            .Where(x => x.Previous > 0 && x.Delta < 0)
            .OrderBy(x => x.Delta)
            .Take(3)
            .ToList();

        var notes = ineffective.Count == 0
            ? "No major drop detected compared with last week."
            : string.Join(" | ", ineffective.Select(x => $"{x.Muscle}: {x.Delta:0.##}"));

        context.WeeklyPerformanceInsights.Add(new WeeklyPerformanceInsight
        {
            Username = effectiveUsername,
            WeekStartDate = startDate,
            WeekEndDate = endDate,
            DaysTrained = currentSessions.Select(s => s.Date.Date).Distinct().Count(),
            TotalSessions = currentSessions.Count,
            PrCount = prCount,
            Notes = notes,
            CreatedAt = DateTime.UtcNow
        });
        await context.SaveChangesAsync();

        return Ok(new WeeklyPerformanceResponse
        {
            WeekStart = startDate,
            WeekEnd = endDate,
            DaysTrained = currentSessions.Select(s => s.Date.Date).Distinct().Count(),
            TotalSessions = currentSessions.Count,
            PrCount = prCount,
            MuscleFrequency = muscleFrequency,
            LessEffectiveMuscles = ineffective.Select(x => new LessEffectiveMuscleItem
            {
                MuscleGroup = x.Muscle.ToString(),
                CurrentAverage = x.Current,
                PreviousAverage = x.Previous,
                Delta = x.Delta
            }).ToList()
        });
    }

    [HttpGet("recovery")]
    public async Task<ActionResult<RecoveryResponse>> GetRecovery(
        [FromQuery] int days = 14,
        [FromQuery] string? username = null)
    {
        var effectiveUsername = ResolveUsername(username);
        if (effectiveUsername == null)
        {
            return Unauthorized("Missing user context");
        }

        var today = DateTime.UtcNow.Date;
        var startDate = today.AddDays(-(Math.Max(days, 7) - 1));

        var sessions = await context.WorkoutSessions
            .Where(s => s.Username == effectiveUsername && s.Date >= startDate && s.Date <= today)
            .OrderByDescending(s => s.Date)
            .ToListAsync();

        var trainedDates = sessions
            .Select(s => s.Date.Date)
            .Distinct()
            .OrderByDescending(d => d)
            .ToList();

        var restDaysLast7 = Enumerable.Range(0, 7)
            .Select(offset => today.AddDays(-offset))
            .Count(date => !trainedDates.Contains(date));

        var consecutiveTrainingDays = 0;
        for (var i = 0; i < 14; i++)
        {
            var date = today.AddDays(-i);
            if (trainedDates.Contains(date))
            {
                consecutiveTrainingDays++;
            }
            else
            {
                break;
            }
        }

        var latestSession = sessions.FirstOrDefault();
        var fatigue = latestSession?.FatigueLevel ?? FatigueLevel.Moderate;

        var latestSessionIds = sessions.Take(2).Select(s => s.Id).ToList();
        var priorSessionIds = sessions.Skip(2).Take(3).Select(s => s.Id).ToList();

        var latestLogs = await context.ExerciseLogs
            .Where(l => latestSessionIds.Contains(l.WorkoutSessionId ?? 0) && !l.IsSkipped && l.Weight.HasValue)
            .Select(l => l.Weight!.Value)
            .ToListAsync();
        var latestAvg = latestLogs.Count > 0 ? latestLogs.Average() : 0m;

        var priorLogs = await context.ExerciseLogs
            .Where(l => priorSessionIds.Contains(l.WorkoutSessionId ?? 0) && !l.IsSkipped && l.Weight.HasValue)
            .Select(l => l.Weight!.Value)
            .ToListAsync();
        var priorAvg = priorLogs.Count > 0 ? priorLogs.Average() : 0m;

        var performanceDrop = priorAvg > 0m && latestAvg + 0.01m < priorAvg;

        string recommendation;
        if (fatigue == FatigueLevel.High || (restDaysLast7 <= 1 && consecutiveTrainingDays >= 4) || performanceDrop)
        {
            recommendation = "Recovery alert: reduce load slightly, prioritize sleep and mobility, and avoid max-effort attempts in the next session.";
        }
        else if (fatigue == FatigueLevel.Moderate)
        {
            recommendation = "Recovery is moderate: keep intensity controlled and monitor soreness before adding overload.";
        }
        else
        {
            recommendation = "Recovery looks good: proceed with planned training and progressive overload as tolerated.";
        }

        if (latestSession != null)
        {
            context.RecoveryInsights.Add(new RecoveryInsight
            {
                Username = effectiveUsername,
                WorkoutSessionId = latestSession.Id,
                FatigueLevel = fatigue,
                RestDaysLast7 = restDaysLast7,
                ConsecutiveTrainingDays = consecutiveTrainingDays,
                Recommendation = recommendation,
                CreatedAt = DateTime.UtcNow
            });
            await context.SaveChangesAsync();
        }

        return Ok(new RecoveryResponse
        {
            FatigueLevel = fatigue.ToString(),
            RestDaysLast7 = restDaysLast7,
            ConsecutiveTrainingDays = consecutiveTrainingDays,
            PerformanceDropDetected = performanceDrop,
            Recommendation = recommendation
        });
    }

    private string? ResolveUsername(string? username)
    {
        var claimUsername = User.FindFirst("username")?.Value;
        return claimUsername ?? username;
    }

    private static DateTime ResolveWeekStart(string? value)
    {
        if (DateTime.TryParse(value, out var parsed))
        {
            return parsed.Date;
        }

        var today = DateTime.UtcNow.Date;
        var diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
        return today.AddDays(-diff);
    }
}

public class ProgressiveOverloadResponse
{
    public string MuscleGroup { get; set; } = string.Empty;
    public string RecommendationType { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
    public decimal? RecommendedIncrement { get; set; }
    public List<ProgressivePoint> Trend { get; set; } = [];
}

public class ProgressivePoint
{
    public int WorkoutSessionId { get; set; }
    public DateTime Date { get; set; }
    public decimal TopWeight { get; set; }
    public decimal AverageWeight { get; set; }
    public decimal Volume { get; set; }
    public int Sets { get; set; }
}

public class WeeklyPerformanceResponse
{
    public DateTime WeekStart { get; set; }
    public DateTime WeekEnd { get; set; }
    public int DaysTrained { get; set; }
    public int TotalSessions { get; set; }
    public int PrCount { get; set; }
    public List<MuscleFrequencyItem> MuscleFrequency { get; set; } = [];
    public List<LessEffectiveMuscleItem> LessEffectiveMuscles { get; set; } = [];
}

public class MuscleFrequencyItem
{
    public string MuscleGroup { get; set; } = string.Empty;
    public int Sessions { get; set; }
}

public class LessEffectiveMuscleItem
{
    public string MuscleGroup { get; set; } = string.Empty;
    public decimal CurrentAverage { get; set; }
    public decimal PreviousAverage { get; set; }
    public decimal Delta { get; set; }
}

public class RecoveryResponse
{
    public string FatigueLevel { get; set; } = string.Empty;
    public int RestDaysLast7 { get; set; }
    public int ConsecutiveTrainingDays { get; set; }
    public bool PerformanceDropDetected { get; set; }
    public string Recommendation { get; set; } = string.Empty;
}

internal class SessionWeightPoint
{
    public int WorkoutSessionId { get; set; }
    public DateTime Date { get; set; }
    public decimal SessionTopWeight { get; set; }
    public decimal SessionAverageWeight { get; set; }
    public decimal SessionVolume { get; set; }
    public int SetsCount { get; set; }
}
