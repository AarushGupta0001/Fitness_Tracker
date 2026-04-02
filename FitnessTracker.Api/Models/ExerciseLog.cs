using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Models;

public class ExerciseLog
{
    public int Id { get; set; }

    public int? WorkoutSessionId { get; set; }

    public WorkoutSession? WorkoutSession { get; set; }

    public int? ExerciseTemplateId { get; set; }

    public ExerciseTemplate? ExerciseTemplate { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public MuscleGroup MuscleGroup { get; set; }

    [Required]
    [MaxLength(120)]
    public string ExerciseName { get; set; } = string.Empty;

    [Range(0, 5)]
    public int SetNumber { get; set; }

    public decimal? Weight { get; set; }

    public bool IsSkipped { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
