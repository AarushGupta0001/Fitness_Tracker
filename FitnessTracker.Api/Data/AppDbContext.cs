using FitnessTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();
    public DbSet<ExerciseTemplate> ExerciseTemplates => Set<ExerciseTemplate>();
    public DbSet<ExerciseLog> ExerciseLogs => Set<ExerciseLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Exercise>()
            .Property(e => e.Weight)
            .HasPrecision(10, 2);

        modelBuilder.Entity<ExerciseLog>()
            .Property(e => e.Weight)
            .HasPrecision(10, 2);

        modelBuilder.Entity<ExerciseTemplate>().HasData(
            new ExerciseTemplate { Id = 1, Name = "Incline Dumbbell Press", MuscleGroup = MuscleGroup.Chest, DisplayOrder = 1 },
            new ExerciseTemplate { Id = 2, Name = "Flat Bench Press", MuscleGroup = MuscleGroup.Chest, DisplayOrder = 2 },
            new ExerciseTemplate { Id = 3, Name = "Seated Butterfly", MuscleGroup = MuscleGroup.Chest, DisplayOrder = 3 },
            new ExerciseTemplate { Id = 4, Name = "Push-ups", MuscleGroup = MuscleGroup.Chest, DisplayOrder = 4 },

            new ExerciseTemplate { Id = 5, Name = "Tricep Pushdown", MuscleGroup = MuscleGroup.Triceps, DisplayOrder = 1 },
            new ExerciseTemplate { Id = 6, Name = "Overhead Dumbbell Extension", MuscleGroup = MuscleGroup.Triceps, DisplayOrder = 2 },
            new ExerciseTemplate { Id = 7, Name = "Single Arm Cable Pushdown", MuscleGroup = MuscleGroup.Triceps, DisplayOrder = 3 },
            new ExerciseTemplate { Id = 8, Name = "Dips", MuscleGroup = MuscleGroup.Triceps, DisplayOrder = 4 },

            new ExerciseTemplate { Id = 9, Name = "Lat Pulldown", MuscleGroup = MuscleGroup.Back, DisplayOrder = 1 },
            new ExerciseTemplate { Id = 10, Name = "Seated Row", MuscleGroup = MuscleGroup.Back, DisplayOrder = 2 },
            new ExerciseTemplate { Id = 11, Name = "One Arm Dumbbell Row", MuscleGroup = MuscleGroup.Back, DisplayOrder = 3 },
            new ExerciseTemplate { Id = 12, Name = "T-Bar Row", MuscleGroup = MuscleGroup.Back, DisplayOrder = 4 },

            new ExerciseTemplate { Id = 13, Name = "Rear Delt Fly", MuscleGroup = MuscleGroup.Shoulder, DisplayOrder = 1 },
            new ExerciseTemplate { Id = 14, Name = "Lateral Raises", MuscleGroup = MuscleGroup.Shoulder, DisplayOrder = 2 },
            new ExerciseTemplate { Id = 15, Name = "Seated Dumbbell Shoulder Press", MuscleGroup = MuscleGroup.Shoulder, DisplayOrder = 3 },
            new ExerciseTemplate { Id = 16, Name = "Shrugs", MuscleGroup = MuscleGroup.Shoulder, DisplayOrder = 4 },

            new ExerciseTemplate { Id = 17, Name = "Barbell Curl", MuscleGroup = MuscleGroup.Biceps, DisplayOrder = 1 },
            new ExerciseTemplate { Id = 18, Name = "Preacher Curl", MuscleGroup = MuscleGroup.Biceps, DisplayOrder = 2 },
            new ExerciseTemplate { Id = 19, Name = "Cable Curl", MuscleGroup = MuscleGroup.Biceps, DisplayOrder = 3 },
            new ExerciseTemplate { Id = 20, Name = "Hammer Curl", MuscleGroup = MuscleGroup.Biceps, DisplayOrder = 4 },

            new ExerciseTemplate { Id = 21, Name = "Leg Press", MuscleGroup = MuscleGroup.Legs, DisplayOrder = 1 },
            new ExerciseTemplate { Id = 22, Name = "Leg Extension", MuscleGroup = MuscleGroup.Legs, DisplayOrder = 2 },
            new ExerciseTemplate { Id = 23, Name = "Hamstring Curl", MuscleGroup = MuscleGroup.Legs, DisplayOrder = 3 },
            new ExerciseTemplate { Id = 24, Name = "Calf Raises", MuscleGroup = MuscleGroup.Legs, DisplayOrder = 4 }
        );
    }
}