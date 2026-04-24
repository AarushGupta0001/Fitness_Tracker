using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FitnessTracker.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseObjectTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseObjectTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    Weight = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Sets = table.Column<int>(type: "INTEGER", nullable: false),
                    Reps = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MuscleGroup = table.Column<int>(type: "INTEGER", nullable: false),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgressiveOverloadInsights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MuscleGroup = table.Column<int>(type: "INTEGER", nullable: false),
                    RecommendationType = table.Column<string>(type: "TEXT", maxLength: 40, nullable: false),
                    Recommendation = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    RecommendedIncrement = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true),
                    SessionIdsCsv = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressiveOverloadInsights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyPerformanceInsights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    WeekStartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    WeekEndDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DaysTrained = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalSessions = table.Column<int>(type: "INTEGER", nullable: false),
                    PrCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyPerformanceInsights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "weight_bar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weight_bar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "weight_db",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weight_db", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "weight_machine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Value = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weight_machine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    DurationMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    MuscleGroup = table.Column<int>(type: "INTEGER", nullable: false),
                    ExerciseObjectTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    DisplayOrder = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseTemplates_ExerciseObjectTypes_ExerciseObjectTypeId",
                        column: x => x.ExerciseObjectTypeId,
                        principalTable: "ExerciseObjectTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    SelectedMuscleGroups = table.Column<string>(type: "TEXT", nullable: false),
                    WorkoutTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    FatigueLevel = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSessions_WorkoutTypes_WorkoutTypeId",
                        column: x => x.WorkoutTypeId,
                        principalTable: "WorkoutTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkoutSessionId = table.Column<int>(type: "INTEGER", nullable: true),
                    ExerciseTemplateId = table.Column<int>(type: "INTEGER", nullable: true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    MuscleGroup = table.Column<int>(type: "INTEGER", nullable: false),
                    ExerciseName = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    SetNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    Weight = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true),
                    IsSkipped = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExerciseLogs_ExerciseTemplates_ExerciseTemplateId",
                        column: x => x.ExerciseTemplateId,
                        principalTable: "ExerciseTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExerciseLogs_WorkoutSessions_WorkoutSessionId",
                        column: x => x.WorkoutSessionId,
                        principalTable: "WorkoutSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PersonalRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    WorkoutSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExerciseTemplateId = table.Column<int>(type: "INTEGER", nullable: false),
                    ExerciseName = table.Column<string>(type: "TEXT", maxLength: 120, nullable: false),
                    MuscleGroup = table.Column<int>(type: "INTEGER", nullable: false),
                    PreviousBestWeight = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    NewBestWeight = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PersonalRecords_ExerciseTemplates_ExerciseTemplateId",
                        column: x => x.ExerciseTemplateId,
                        principalTable: "ExerciseTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PersonalRecords_WorkoutSessions_WorkoutSessionId",
                        column: x => x.WorkoutSessionId,
                        principalTable: "WorkoutSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecoveryInsights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    WorkoutSessionId = table.Column<int>(type: "INTEGER", nullable: false),
                    FatigueLevel = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    RestDaysLast7 = table.Column<int>(type: "INTEGER", nullable: false),
                    ConsecutiveTrainingDays = table.Column<int>(type: "INTEGER", nullable: false),
                    Recommendation = table.Column<string>(type: "TEXT", maxLength: 600, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecoveryInsights", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecoveryInsights_WorkoutSessions_WorkoutSessionId",
                        column: x => x.WorkoutSessionId,
                        principalTable: "WorkoutSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ExerciseObjectTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "dumbel" },
                    { 2, "bar" },
                    { 3, "machine" }
                });

            migrationBuilder.InsertData(
                table: "WorkoutTypes",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "push", "Push" },
                    { 2, "pull", "Pull" },
                    { 3, "leg-shoulder", "Leg & Shoulder" },
                    { 4, "upper", "Mixed Upper" },
                    { 5, "lower", "Legs" }
                });

            migrationBuilder.InsertData(
                table: "weight_bar",
                columns: new[] { "Id", "DisplayOrder", "Value" },
                values: new object[,]
                {
                    { 1, 1, 5m },
                    { 2, 2, 7.5m },
                    { 3, 3, 10m },
                    { 4, 4, 12.5m },
                    { 5, 5, 15m },
                    { 6, 6, 17.5m },
                    { 7, 7, 20m },
                    { 8, 8, 22.5m },
                    { 9, 9, 25m },
                    { 10, 10, 27.5m },
                    { 11, 11, 30m },
                    { 12, 12, 32.5m },
                    { 13, 13, 35m }
                });

            migrationBuilder.InsertData(
                table: "weight_db",
                columns: new[] { "Id", "DisplayOrder", "Value" },
                values: new object[,]
                {
                    { 1, 1, 2.5m },
                    { 2, 2, 5m },
                    { 3, 3, 7.5m },
                    { 4, 4, 10m },
                    { 5, 5, 12m },
                    { 6, 6, 12.5m },
                    { 7, 7, 14m },
                    { 8, 8, 15m },
                    { 9, 9, 16m },
                    { 10, 10, 17.5m },
                    { 11, 11, 20m },
                    { 12, 12, 24m }
                });

            migrationBuilder.InsertData(
                table: "weight_machine",
                columns: new[] { "Id", "DisplayOrder", "Value" },
                values: new object[,]
                {
                    { 1, 1, 5m },
                    { 2, 2, 10m },
                    { 3, 3, 15m },
                    { 4, 4, 20m },
                    { 5, 5, 25m },
                    { 6, 6, 30m },
                    { 7, 7, 35m },
                    { 8, 8, 40m },
                    { 9, 9, 45m },
                    { 10, 10, 50m },
                    { 11, 11, 55m },
                    { 12, 12, 60m },
                    { 13, 13, 65m },
                    { 14, 14, 70m },
                    { 15, 15, 75m },
                    { 16, 16, 80m },
                    { 17, 17, 85m },
                    { 18, 18, 90m },
                    { 19, 19, 95m },
                    { 20, 20, 100m }
                });

            migrationBuilder.InsertData(
                table: "ExerciseTemplates",
                columns: new[] { "Id", "DisplayOrder", "ExerciseObjectTypeId", "MuscleGroup", "Name" },
                values: new object[,]
                {
                    { 1, 1, 1, 0, "Incline Dumbbell Press" },
                    { 2, 2, 2, 0, "Flat Bench Press" },
                    { 3, 3, 3, 0, "Seated Butterfly" },
                    { 4, 4, 1, 0, "Push-ups" },
                    { 5, 1, 3, 1, "Tricep Pushdown" },
                    { 6, 2, 1, 1, "Overhead Dumbbell Extension" },
                    { 7, 3, 3, 1, "Single Arm Cable Pushdown" },
                    { 8, 4, 2, 1, "Dips" },
                    { 9, 1, 3, 2, "Lat Pulldown" },
                    { 10, 2, 3, 2, "Seated Row" },
                    { 11, 3, 1, 2, "One Arm Dumbbell Row" },
                    { 12, 4, 2, 2, "T-Bar Row" },
                    { 13, 1, 3, 3, "Rear Delt Fly" },
                    { 14, 2, 1, 3, "Lateral Raises" },
                    { 15, 3, 1, 3, "Seated Dumbbell Shoulder Press" },
                    { 16, 4, 2, 3, "Shrugs" },
                    { 17, 1, 2, 4, "Barbell Curl" },
                    { 18, 2, 3, 4, "Preacher Curl" },
                    { 19, 3, 3, 4, "Cable Curl" },
                    { 20, 4, 1, 4, "Hammer Curl" },
                    { 21, 1, 3, 5, "Leg Press" },
                    { 22, 2, 3, 5, "Leg Extension" },
                    { 23, 3, 3, 5, "Hamstring Curl" },
                    { 24, 4, 2, 5, "Calf Raises" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_ExerciseTemplateId",
                table: "ExerciseLogs",
                column: "ExerciseTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_WorkoutSessionId",
                table: "ExerciseLogs",
                column: "WorkoutSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseObjectTypes_Name",
                table: "ExerciseObjectTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseTemplates_ExerciseObjectTypeId",
                table: "ExerciseTemplates",
                column: "ExerciseObjectTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalRecords_ExerciseTemplateId",
                table: "PersonalRecords",
                column: "ExerciseTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_PersonalRecords_Username_ExerciseTemplateId_Date",
                table: "PersonalRecords",
                columns: new[] { "Username", "ExerciseTemplateId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_PersonalRecords_WorkoutSessionId",
                table: "PersonalRecords",
                column: "WorkoutSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressiveOverloadInsights_Username_MuscleGroup_CreatedAt",
                table: "ProgressiveOverloadInsights",
                columns: new[] { "Username", "MuscleGroup", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RecoveryInsights_Username_CreatedAt",
                table: "RecoveryInsights",
                columns: new[] { "Username", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RecoveryInsights_WorkoutSessionId",
                table: "RecoveryInsights",
                column: "WorkoutSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyPerformanceInsights_Username_WeekStartDate",
                table: "WeeklyPerformanceInsights",
                columns: new[] { "Username", "WeekStartDate" });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSessions_WorkoutTypeId",
                table: "WorkoutSessions",
                column: "WorkoutTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutTypes_Code",
                table: "WorkoutTypes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseLogs");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "PersonalRecords");

            migrationBuilder.DropTable(
                name: "ProgressiveOverloadInsights");

            migrationBuilder.DropTable(
                name: "RecoveryInsights");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "WeeklyPerformanceInsights");

            migrationBuilder.DropTable(
                name: "weight_bar");

            migrationBuilder.DropTable(
                name: "weight_db");

            migrationBuilder.DropTable(
                name: "weight_machine");

            migrationBuilder.DropTable(
                name: "Workouts");

            migrationBuilder.DropTable(
                name: "ExerciseTemplates");

            migrationBuilder.DropTable(
                name: "WorkoutSessions");

            migrationBuilder.DropTable(
                name: "ExerciseObjectTypes");

            migrationBuilder.DropTable(
                name: "WorkoutTypes");
        }
    }
}
