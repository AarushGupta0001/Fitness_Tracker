using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Models;

public class WeeklyPerformanceInsight
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    public DateTime WeekStartDate { get; set; }

    [Required]
    public DateTime WeekEndDate { get; set; }

    public int DaysTrained { get; set; }

    public int TotalSessions { get; set; }

    public int PrCount { get; set; }

    [MaxLength(1000)]
    public string Notes { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}