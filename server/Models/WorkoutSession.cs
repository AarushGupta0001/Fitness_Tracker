using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Models;

public class WorkoutSession
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public DateTime Date { get; set; }

    [Required]
    public string SelectedMuscleGroups { get; set; } = string.Empty; // Stored as JSON or comma-separated

    [Required]
    public int WorkoutTypeId { get; set; }

    public WorkoutType? WorkoutType { get; set; }

    public FatigueLevel? FatigueLevel { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
