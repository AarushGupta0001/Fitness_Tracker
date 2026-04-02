using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Models;

public class Workout
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Range(1, 480)]
    public int DurationMinutes { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [MaxLength(500)]
    public string? Notes { get; set; }
}