using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessTracker.Api.Models;

public class Exercise
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Weight { get; set; }

    [Range(1, 100)]
    public int Sets { get; set; }

    [Range(1, 100)]
    public int Reps { get; set; }

    [Required]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    public MuscleGroup MuscleGroup { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;
}
