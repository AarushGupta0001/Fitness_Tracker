using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Models;

public class ExerciseLog
{
    public int Id { get; set; }

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

    [Range(1, 5)]
    public int SetNumber { get; set; }

    [Required]
    public decimal Weight { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
