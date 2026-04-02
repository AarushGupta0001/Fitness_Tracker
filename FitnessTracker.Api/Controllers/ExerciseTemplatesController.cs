using FitnessTracker.Api.Data;
using FitnessTracker.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExerciseTemplatesController(AppDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ExerciseTemplateResponse>>> GetExerciseTemplates([FromQuery] string? groups)
    {
        var query = context.ExerciseTemplates.AsQueryable();

        if (!string.IsNullOrWhiteSpace(groups))
        {
            var parsedGroups = groups
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(ParseMuscleGroup)
                .Where(g => g.HasValue)
                .Select(g => g!.Value)
                .ToHashSet();

            if (parsedGroups.Count == 0)
            {
                return BadRequest("No valid muscle groups were provided.");
            }

            query = query.Where(e => parsedGroups.Contains(e.MuscleGroup));
        }

        var templates = await query
            .OrderBy(e => e.MuscleGroup)
            .ThenBy(e => e.DisplayOrder)
            .Select(e => new ExerciseTemplateResponse
            {
                Id = e.Id,
                Name = e.Name,
                MuscleGroup = e.MuscleGroup.ToString()
            })
            .ToListAsync();

        return Ok(templates);
    }

    private static MuscleGroup? ParseMuscleGroup(string value)
    {
        if (Enum.TryParse<MuscleGroup>(value, true, out var fromName))
        {
            return fromName;
        }

        if (int.TryParse(value, out var fromNumber) && Enum.IsDefined(typeof(MuscleGroup), fromNumber))
        {
            return (MuscleGroup)fromNumber;
        }

        return null;
    }
}

public class ExerciseTemplateResponse
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string MuscleGroup { get; set; } = string.Empty;
}
