using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Models;

public class ProgressiveOverloadInsight
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public MuscleGroup MuscleGroup { get; set; }

    [Required]
    [MaxLength(40)]
    public string RecommendationType { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Recommendation { get; set; } = string.Empty;

    public decimal? RecommendedIncrement { get; set; }

    [MaxLength(200)]
    public string SessionIdsCsv { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}