using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Models;

public class ExerciseTemplate
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public MuscleGroup MuscleGroup { get; set; }

    [Required]
    public int ExerciseObjectTypeId { get; set; }

    public ExerciseObjectType? ExerciseObjectType { get; set; }

    public int DisplayOrder { get; set; }
}
