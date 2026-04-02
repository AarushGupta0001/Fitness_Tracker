using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Models;

public class ExerciseObjectType
{
    public int Id { get; set; }

    [Required]
    [MaxLength(40)]
    public string Name { get; set; } = string.Empty;
}
