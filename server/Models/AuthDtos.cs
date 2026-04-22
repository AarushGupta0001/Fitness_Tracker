using System.ComponentModel.DataAnnotations;

namespace FitnessTracker.Api.Models;

public record RegisterRequest(
    [Required][MaxLength(50)] string Username,
    [Required][MinLength(6)] string Password
);

public record LoginRequest(
    [Required][MaxLength(50)] string Username,
    [Required] string Password
);

public record AuthResponse(string Token, string Username, DateTime ExpiresAt);
