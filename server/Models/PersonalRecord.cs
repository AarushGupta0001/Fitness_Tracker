using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Models;

public class PersonalRecord
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public int WorkoutSessionId { get; set; }

    public WorkoutSession? WorkoutSession { get; set; }

    [Required]
    public int ExerciseTemplateId { get; set; }

    public ExerciseTemplate? ExerciseTemplate { get; set; }

    [Required]
    [MaxLength(120)]
    public string ExerciseName { get; set; } = string.Empty;

    [Required]
    public MuscleGroup MuscleGroup { get; set; }

    [Required]
    public decimal PreviousBestWeight { get; set; }

    [Required]
    public decimal NewBestWeight { get; set; }

    [Required]
    public DateTime Date { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}