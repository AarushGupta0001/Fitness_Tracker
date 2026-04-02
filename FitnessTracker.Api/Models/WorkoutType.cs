using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Models;

public class WorkoutType
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
