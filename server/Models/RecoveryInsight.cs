using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Models;

public class RecoveryInsight
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public int WorkoutSessionId { get; set; }

    public WorkoutSession? WorkoutSession { get; set; }

    [Required]
    public FatigueLevel FatigueLevel { get; set; }

    public int RestDaysLast7 { get; set; }

    public int ConsecutiveTrainingDays { get; set; }

    [Required]
    [MaxLength(600)]
    public string Recommendation { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}