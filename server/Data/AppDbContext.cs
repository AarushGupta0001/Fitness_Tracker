using FitnessTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FitnessTracker.Api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<WorkoutSession> WorkoutSessions => Set<WorkoutSession>();
    public DbSet<ExerciseTemplate> ExerciseTemplates => Set<ExerciseTemplate>();
    public DbSet<ExerciseLog> ExerciseLogs => Set<ExerciseLog>();
    public DbSet<ExerciseObjectType> ExerciseObjectTypes => Set<ExerciseObjectType>();
    public DbSet<WeightDb> WeightDb => Set<WeightDb>();
    public DbSet<WeightBar> WeightBar => Set<WeightBar>();
    public DbSet<WeightMachine> WeightMachine => Set<WeightMachine>();
    public DbSet<WorkoutType> WorkoutTypes => Set<WorkoutType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Exercise>()
            .Property(e => e.Weight)
            .HasPrecision(10, 2);

        modelBuilder.Entity<ExerciseLog>()
            .Property(e => e.Weight)
            .HasPrecision(10, 2);

        modelBuilder.Entity<WeightDb>()
            .Property(w => w.Value)
            .HasPrecision(10, 2);

        modelBuilder.Entity<WeightBar>()
            .Property(w => w.Value)
            .HasPrecision(10, 2);

        modelBuilder.Entity<WeightMachine>()
            .Property(w => w.Value)
            .HasPrecision(10, 2);

        modelBuilder.Entity<WeightDb>().ToTable("weight_db");
        modelBuilder.Entity<WeightBar>().ToTable("weight_bar");
        modelBuilder.Entity<WeightMachine>().ToTable("weight_machine");

        modelBuilder.Entity<WorkoutType>()
            .HasIndex(w => w.Code)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<ExerciseObjectType>()
            .HasIndex(o => o.Name)
            .IsUnique();

        modelBuilder.Entity<ExerciseTemplate>()
            .HasOne(e => e.ExerciseObjectType)
            .WithMany()
            .HasForeignKey(e => e.ExerciseObjectTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WorkoutSession>()
            .HasOne(w => w.WorkoutType)
            .WithMany()
            .HasForeignKey(w => w.WorkoutTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ExerciseLog>()
            .HasOne(e => e.WorkoutSession)
            .WithMany()
            .HasForeignKey(e => e.WorkoutSessionId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ExerciseLog>()
            .HasOne(e => e.ExerciseTemplate)
            .WithMany()
            .HasForeignKey(e => e.ExerciseTemplateId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ExerciseObjectType>().HasData(
            new ExerciseObjectType { Id = 1, Name = "dumbel" },
            new ExerciseObjectType { Id = 2, Name = "bar" },
            new ExerciseObjectType { Id = 3, Name = "machine" }
        );

        modelBuilder.Entity<WorkoutType>().HasData(
            new WorkoutType { Id = 1, Code = "push", Name = "Push" },
            new WorkoutType { Id = 2, Code = "pull", Name = "Pull" },
            new WorkoutType { Id = 3, Code = "leg-shoulder", Name = "Leg & Shoulder" },
            new WorkoutType { Id = 4, Code = "upper", Name = "Mixed Upper" },
            new WorkoutType { Id = 5, Code = "lower", Name = "Legs" }
        );

        modelBuilder.Entity<WeightDb>().HasData(
            new WeightDb { Id = 1, Value = 2.5m, DisplayOrder = 1 },
            new WeightDb { Id = 2, Value = 5m, DisplayOrder = 2 },
            new WeightDb { Id = 3, Value = 7.5m, DisplayOrder = 3 },
            new WeightDb { Id = 4, Value = 10m, DisplayOrder = 4 },
            new WeightDb { Id = 5, Value = 12m, DisplayOrder = 5 },
            new WeightDb { Id = 6, Value = 12.5m, DisplayOrder = 6 },
            new WeightDb { Id = 7, Value = 14m, DisplayOrder = 7 },
            new WeightDb { Id = 8, Value = 15m, DisplayOrder = 8 },
            new WeightDb { Id = 9, Value = 16m, DisplayOrder = 9 },
            new WeightDb { Id = 10, Value = 17.5m, DisplayOrder = 10 },
            new WeightDb { Id = 11, Value = 20m, DisplayOrder = 11 },
            new WeightDb { Id = 12, Value = 24m, DisplayOrder = 12 }
        );

        modelBuilder.Entity<WeightBar>().HasData(
            new WeightBar { Id = 1, Value = 5m, DisplayOrder = 1 },
            new WeightBar { Id = 2, Value = 7.5m, DisplayOrder = 2 },
            new WeightBar { Id = 3, Value = 10m, DisplayOrder = 3 },
            new WeightBar { Id = 4, Value = 12.5m, DisplayOrder = 4 },
            new WeightBar { Id = 5, Value = 15m, DisplayOrder = 5 },
            new WeightBar { Id = 6, Value = 17.5m, DisplayOrder = 6 },
            new WeightBar { Id = 7, Value = 20m, DisplayOrder = 7 },
            new WeightBar { Id = 8, Value = 22.5m, DisplayOrder = 8 },
            new WeightBar { Id = 9, Value = 25m, DisplayOrder = 9 },
            new WeightBar { Id = 10, Value = 27.5m, DisplayOrder = 10 },
            new WeightBar { Id = 11, Value = 30m, DisplayOrder = 11 },
            new WeightBar { Id = 12, Value = 32.5m, DisplayOrder = 12 },
            new WeightBar { Id = 13, Value = 35m, DisplayOrder = 13 }
        );

        modelBuilder.Entity<WeightMachine>().HasData(
            new WeightMachine { Id = 1, Value = 5m, DisplayOrder = 1 },
            new WeightMachine { Id = 2, Value = 10m, DisplayOrder = 2 },
            new WeightMachine { Id = 3, Value = 15m, DisplayOrder = 3 },
            new WeightMachine { Id = 4, Value = 20m, DisplayOrder = 4 },
            new WeightMachine { Id = 5, Value = 25m, DisplayOrder = 5 },
            new WeightMachine { Id = 6, Value = 30m, DisplayOrder = 6 },
            new WeightMachine { Id = 7, Value = 35m, DisplayOrder = 7 },
            new WeightMachine { Id = 8, Value = 40m, DisplayOrder = 8 },
            new WeightMachine { Id = 9, Value = 45m, DisplayOrder = 9 },
            new WeightMachine { Id = 10, Value = 50m, DisplayOrder = 10 },
            new WeightMachine { Id = 11, Value = 55m, DisplayOrder = 11 },
            new WeightMachine { Id = 12, Value = 60m, DisplayOrder = 12 },
            new WeightMachine { Id = 13, Value = 65m, DisplayOrder = 13 },
            new WeightMachine { Id = 14, Value = 70m, DisplayOrder = 14 },
            new WeightMachine { Id = 15, Value = 75m, DisplayOrder = 15 },
            new WeightMachine { Id = 16, Value = 80m, DisplayOrder = 16 },
            new WeightMachine { Id = 17, Value = 85m, DisplayOrder = 17 },
            new WeightMachine { Id = 18, Value = 90m, DisplayOrder = 18 },
            new WeightMachine { Id = 19, Value = 95m, DisplayOrder = 19 },
            new WeightMachine { Id = 20, Value = 100m, DisplayOrder = 20 }
        );

        modelBuilder.Entity<ExerciseTemplate>().HasData(
            new ExerciseTemplate { Id = 1, Name = "Incline Dumbbell Press", MuscleGroup = MuscleGroup.Chest, ExerciseObjectTypeId = 1, DisplayOrder = 1 },
            new ExerciseTemplate { Id = 2, Name = "Flat Bench Press", MuscleGroup = MuscleGroup.Chest, ExerciseObjectTypeId = 2, DisplayOrder = 2 },
            new ExerciseTemplate { Id = 3, Name = "Seated Butterfly", MuscleGroup = MuscleGroup.Chest, ExerciseObjectTypeId = 3, DisplayOrder = 3 },
            new ExerciseTemplate { Id = 4, Name = "Push-ups", MuscleGroup = MuscleGroup.Chest, ExerciseObjectTypeId = 1, DisplayOrder = 4 },

            new ExerciseTemplate { Id = 5, Name = "Tricep Pushdown", MuscleGroup = MuscleGroup.Triceps, ExerciseObjectTypeId = 3, DisplayOrder = 1 },
            new ExerciseTemplate { Id = 6, Name = "Overhead Dumbbell Extension", MuscleGroup = MuscleGroup.Triceps, ExerciseObjectTypeId = 1, DisplayOrder = 2 },
            new ExerciseTemplate { Id = 7, Name = "Single Arm Cable Pushdown", MuscleGroup = MuscleGroup.Triceps, ExerciseObjectTypeId = 3, DisplayOrder = 3 },
            new ExerciseTemplate { Id = 8, Name = "Dips", MuscleGroup = MuscleGroup.Triceps, ExerciseObjectTypeId = 2, DisplayOrder = 4 },

            new ExerciseTemplate { Id = 9, Name = "Lat Pulldown", MuscleGroup = MuscleGroup.Back, ExerciseObjectTypeId = 3, DisplayOrder = 1 },
            new ExerciseTemplate { Id = 10, Name = "Seated Row", MuscleGroup = MuscleGroup.Back, ExerciseObjectTypeId = 3, DisplayOrder = 2 },
            new ExerciseTemplate { Id = 11, Name = "One Arm Dumbbell Row", MuscleGroup = MuscleGroup.Back, ExerciseObjectTypeId = 1, DisplayOrder = 3 },
            new ExerciseTemplate { Id = 12, Name = "T-Bar Row", MuscleGroup = MuscleGroup.Back, ExerciseObjectTypeId = 2, DisplayOrder = 4 },

            new ExerciseTemplate { Id = 13, Name = "Rear Delt Fly", MuscleGroup = MuscleGroup.Shoulder, ExerciseObjectTypeId = 3, DisplayOrder = 1 },
            new ExerciseTemplate { Id = 14, Name = "Lateral Raises", MuscleGroup = MuscleGroup.Shoulder, ExerciseObjectTypeId = 1, DisplayOrder = 2 },
            new ExerciseTemplate { Id = 15, Name = "Seated Dumbbell Shoulder Press", MuscleGroup = MuscleGroup.Shoulder, ExerciseObjectTypeId = 1, DisplayOrder = 3 },
            new ExerciseTemplate { Id = 16, Name = "Shrugs", MuscleGroup = MuscleGroup.Shoulder, ExerciseObjectTypeId = 2, DisplayOrder = 4 },

            new ExerciseTemplate { Id = 17, Name = "Barbell Curl", MuscleGroup = MuscleGroup.Biceps, ExerciseObjectTypeId = 2, DisplayOrder = 1 },
            new ExerciseTemplate { Id = 18, Name = "Preacher Curl", MuscleGroup = MuscleGroup.Biceps, ExerciseObjectTypeId = 3, DisplayOrder = 2 },
            new ExerciseTemplate { Id = 19, Name = "Cable Curl", MuscleGroup = MuscleGroup.Biceps, ExerciseObjectTypeId = 3, DisplayOrder = 3 },
            new ExerciseTemplate { Id = 20, Name = "Hammer Curl", MuscleGroup = MuscleGroup.Biceps, ExerciseObjectTypeId = 1, DisplayOrder = 4 },

            new ExerciseTemplate { Id = 21, Name = "Leg Press", MuscleGroup = MuscleGroup.Legs, ExerciseObjectTypeId = 3, DisplayOrder = 1 },
            new ExerciseTemplate { Id = 22, Name = "Leg Extension", MuscleGroup = MuscleGroup.Legs, ExerciseObjectTypeId = 3, DisplayOrder = 2 },
            new ExerciseTemplate { Id = 23, Name = "Hamstring Curl", MuscleGroup = MuscleGroup.Legs, ExerciseObjectTypeId = 3, DisplayOrder = 3 },
            new ExerciseTemplate { Id = 24, Name = "Calf Raises", MuscleGroup = MuscleGroup.Legs, ExerciseObjectTypeId = 2, DisplayOrder = 4 }
        );
    }
}